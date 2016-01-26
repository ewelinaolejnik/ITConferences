using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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
using ITConferences.WebUI.Extensions;

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
        private IGenericRepository<Image> _imageRepository;
        private IFilterConferenceHelper _conferenceFilter;

        public IEnumerable<Conference> Conferences { get; private set; }

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
            IGenericRepository<Tag> tagRepository, IGenericRepository<City> cityRepository,
            IFilterConferenceHelper conferenceFilter, IGenericRepository<Image> imageRepository)
        {
            if (conferenceRepository == null || countryRepository == null || tagRepository == null || cityRepository == null || imageRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }
            if (conferenceFilter == null)
            {
                throw new ArgumentNullException("Conference filter is null!");
            }

            _conferenceRepository = conferenceRepository;
            _countryRepository = countryRepository;
            _tagRepository = tagRepository;
            _cityRepository = cityRepository;
            _imageRepository = imageRepository;
            Conferences = _conferenceRepository.GetAll().ToList();
            _conferenceFilter = conferenceFilter;
            _conferenceFilter.Conferences = Conferences;
        }
        #endregion

        #region Index
        //TODO: unit tests!
        // GET: Conferences
        public ActionResult Index(string nameFilter)
        {
            ViewData["TagsFilter"] = new MultiSelectList(_tagRepository.GetAll(), "TagID", "Name");
            _conferenceFilter.FilterByName(ViewData, nameFilter);

            var pageSize = GetPageSize(0);
            var pagedConfs =
                _conferenceFilter.Conferences.ToList().GetRange(0, pageSize);
            return View(pagedConfs);
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

        //TODO: unit tests!
        public PartialViewResult GetConferences(string nameFilter, string locationFilter, int[] selectedTagsIds, DateFilter? dateFilter, int? page)
        {
            _conferenceFilter.Conferences = Conferences;
            _conferenceFilter.FilterByName(ViewData, nameFilter);
            _conferenceFilter.FilterByLocation(ViewData, locationFilter);
            _conferenceFilter.FilterByTags(ViewData, selectedTagsIds, Tags);
            if (dateFilter != null)
                _conferenceFilter.FilterByTime(ViewData, dateFilter.Value, Conferences);

            var pageId = page ?? 0;
            var pageSize = GetPageSize(pageId);

            if (pageId * PageSize >= _conferenceFilter.Conferences.Count())
            {
                return null;
            }

            var pagedConfs =
                 _conferenceFilter.Conferences.ToList().GetRange(pageId * PageSize, pageSize);
            return PartialView("_ConferencesView", pagedConfs.OrderBy(e => e.Date).ToList());
        }

        //TODO: unit tests!
        private int GetPageSize(int pageId)
        {
            return (PageSize * pageId) + PageSize < _conferenceFilter.Conferences.Count()
               ? PageSize
               : Math.Abs(_conferenceFilter.Conferences.Count() - (PageSize * pageId));
        }
        #endregion

        // GET: Conferences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Conference conference = _conferenceRepository.GetById(id);

            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        #region Create
        //TODO: unit tests!
        // GET: Conferences/Create
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
            {
                Danger("Log in to add an event, please", true);

                return RedirectToAction("Login", "Account");
            }

            ViewData["TargetCountryId"] = new SelectList(Countries, "CountryID", "Name");
            ViewData["TagsSelector"] = new MultiSelectList(Tags, "TagID", "Name");
            return View();
        }

        //TODO: unit tests!
        // POST: Conferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(int[] tags, Conference conference, HttpPostedFileBase image)
        {
            ViewData["TagsSelector"] = new MultiSelectList(Tags, "TagID", "Name");
            ViewData["TargetCountryId"] = new SelectList(Countries, "CountryID", "Name");

            try
            {
                _conferenceRepository.InsertAndSubmit(conference);
                if (tags != null)
                {
                    var selectedTags = Tags.Where(e => tags.Contains(e.TagID)).ToList();
                    conference.Tags.ToList().AddRange(selectedTags);
                }
                var img = new Image()
                {
                    ConcreteImage = image.InputStream.ToByteArray()
                };
                _imageRepository.InsertAndSubmit(img);
                conference.Image = img;
                _conferenceRepository.UpdateAndSubmit(conference);
                Success("Great job, You added the event!", true);
                return View(conference);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View();
            }
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase image)
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];


                return View("Create");
            }

            return View("Create");
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
