﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;
using ITConferences.WebUI.Helpers;
using WebGrease.Css.Extensions;


namespace ITConferences.WebUI.Controllers
{
    public class ConferencesController : BaseController
    {
        #region Fields
        private const int PageSize = 15;

        private IGenericRepository<Conference> _conferenceRepository;
        private IGenericRepository<Country> _countryRepository;
        private IGenericRepository<Tag> _tagRepository;
        private IGenericRepository<City> _cityRepository;
        private IFilterConferenceHelper _conferenceFilter;
        private IControllerHelper _controllerHelper;

        public IEnumerable<Conference> Conferences { get; private set; }
        public IEnumerable<Conference> PagedConferences { get; private set; }

        public IEnumerable<Country> Countries
        {
            get { return _countryRepository.GetAll().ToList(); }
        }

        public IEnumerable<Tag> Tags
        {
            get { return _tagRepository.GetAll().ToList(); }
        }

        #endregion

        #region Ctor
        public ConferencesController(IGenericRepository<Conference> conferenceRepository, IGenericRepository<Country> countryRepository,
            IGenericRepository<Tag> tagRepository, IGenericRepository<City> cityRepository, IFilterConferenceHelper conferenceFilter, 
            IGenericRepository<Image> imageRepository, IControllerHelper controllerHelper) :base(imageRepository)
        {
            if (conferenceRepository == null || countryRepository == null || tagRepository == null ||
                cityRepository == null || imageRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }
            if (conferenceFilter == null || controllerHelper == null)
            {
                throw new ArgumentNullException("Some helper is null!");
            }

            _conferenceRepository = conferenceRepository;
            _countryRepository = countryRepository;
            _tagRepository = tagRepository;
            _cityRepository = cityRepository;
            _conferenceFilter = conferenceFilter;
            _controllerHelper = controllerHelper;

            Conferences = _conferenceRepository.GetAll().ToList();
            _conferenceFilter.Conferences = Conferences.OrderBy(e => e.Date);
        }
        #endregion

        #region Index
        // GET: Conferences
        public ActionResult Index(string nameFilter, int? tagsFilter)
        {
            _conferenceFilter.FilterByName(ViewData, nameFilter);
            _conferenceFilter.FilterByTags(ViewData, new int[] { tagsFilter ?? 0 }, Tags); //TODO: filter by tags

            ViewData["TagsFilter"] = new MultiSelectList(_tagRepository.GetAll(), "TagID", "Name");
            ViewData["ResultsCount"] = _controllerHelper.GetResultsCount(_conferenceFilter.Conferences.Count());

            var pageSize = _controllerHelper.GetPageSize(0, PageSize, _conferenceFilter.Conferences.Count());
            PagedConferences =
                _conferenceFilter.Conferences.ToList().GetRange(0, pageSize);
            return View(PagedConferences);
        }

        public JsonResult GetLocations(string locationFilter)
        {
            var cities = _cityRepository.GetAll().ToList();
            var filteredLocation =
                cities.Where(
                    e =>
                        e.Name.ToLower().StartsWith(locationFilter.ToLower()) ||
                        e.Country.Name.ToLower().StartsWith(locationFilter.ToLower())).ToList();

            List<string> result = new List<string>();
            filteredLocation.ForEach(e => result.Add(e.Name + ", " + e.Country.Name));

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public PartialViewResult GetConferences(string nameFilter, string locationFilter, int[] selectedTagsIds, DateFilter? dateFilter, int? page, bool filter = false)
        {
            _conferenceFilter.Conferences = Conferences.OrderBy(e => e.Date);
            _conferenceFilter.FilterByName(ViewData, nameFilter);
            _conferenceFilter.FilterByLocation(ViewData, locationFilter);
            _conferenceFilter.FilterByTags(ViewData, selectedTagsIds, Tags);
            if (dateFilter != null)
                _conferenceFilter.FilterByTime(ViewData, dateFilter.Value, _conferenceFilter.Conferences);

            if (_conferenceFilter.Conferences.Count() == 0)
                return PartialView("_NoResuls");

            if ((page ?? 0) * PageSize >= _conferenceFilter.Conferences.Count())
                return null;

            ViewData["ResultsCount"] = _controllerHelper.GetResultsCount(_conferenceFilter.Conferences.Count(), !filter);
            
            var pageSize = _controllerHelper.GetPageSize(page ?? 0, PageSize, _conferenceFilter.Conferences.Count());
            PagedConferences =
                 _conferenceFilter.Conferences.ToList().GetRange((page ?? 0) * PageSize, pageSize);
            return PartialView("_ConferencesView", PagedConferences.ToList());
        }
        
        #endregion

        #region Details
        // GET: Conferences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Conference conference = _conferenceRepository.GetById(id);

            if (conference == null)
                return HttpNotFound();

            return View(conference);
        }

        public ActionResult AddEvaluation(int conferenceId, int countOfStars, string comment, string ownerId)
        {
            if (!Request.IsAuthenticated)
                GetLoginMessage("Log in to add evaluation, please");

            Conference conference = _conferenceRepository.GetById(conferenceId);

            if (conference == null)
                return HttpNotFound();

            if (string.IsNullOrWhiteSpace(comment))
                GetCommentMessage(conference);

            var eval = _controllerHelper.GetEvaluation(ownerId, comment, countOfStars);
            conference.Evaluation.Add(eval);
            _conferenceRepository.UpdateAndSubmit();

            return View("Details", conference);
        }

        #endregion

        #region Create
        // GET: Conferences/Create
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
                GetLoginMessage("Log in to add an event, please");

            ViewData["TargetCountryId"] = new SelectList(Countries, "CountryID", "Name");
            ViewData["TagsSelector"] = new MultiSelectList(Tags, "TagID", "Name");
            return View();
        }
        
        // POST: Conferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string tags, Conference conference, HttpPostedFileBase image, string userId)
        {
            ViewData["TagsSelector"] = new MultiSelectList(Tags, "TagID", "Name");
            ViewData["TargetCountryId"] = new SelectList(Countries, "CountryID", "Name");

            try
            {
                if (image != null)
                    _controllerHelper.AssignImage(image, conference);

                if (!string.IsNullOrWhiteSpace(userId))
                    _controllerHelper.AssignOrganizer(userId, conference);

                _conferenceRepository.InsertAndSubmit(conference);

                if (tags != "null")
                    _controllerHelper.AssignTags(tags, Tags, _tagRepository, conference);

                Success("Great job, You added the event!", true);
                return View(conference);
            }
            catch (Exception dbEx)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View();
            }
        }

        public JsonResult GetSelectedCities(int countryId)
        {
            var country = _countryRepository.GetById(countryId);
            var selectedCities = country.Cities.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.CityID.ToString()
            });

            return Json(selectedCities, JsonRequestBehavior.AllowGet);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _conferenceRepository.DisposeDataContext();
                _countryRepository.DisposeDataContext();
                _cityRepository.DisposeDataContext();
                _tagRepository.DisposeDataContext();
            }
            base.Dispose(disposing);
        }
    }
}
