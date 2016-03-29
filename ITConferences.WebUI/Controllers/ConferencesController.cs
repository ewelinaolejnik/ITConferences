using System;
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
        private const int PageSize = 21;
       
        private IFilterConferenceHelper _conferenceFilter;
        private IControllerHelper _controllerHelper;

        public IEnumerable<Conference> Conferences { get; private set; }
        public IEnumerable<Conference> PagedConferences { get; private set; }

        public IEnumerable<Country> Countries
        {
            get { return _repository.GetAll<Country>().ToList(); }
        }

        public IEnumerable<Tag> Tags
        {
            get { return _repository.GetAll<Tag>().ToList(); }
        }

        public IEnumerable<City> Cities
        {
            get { return _repository.GetAll<City>(); }
        }

        #endregion

        #region Ctor
        public ConferencesController(IFilterConferenceHelper conferenceFilter,
            IGenericRepository repository, IControllerHelper controllerHelper) : base(repository)
        {
            if (conferenceFilter == null || controllerHelper == null)
            {
                throw new ArgumentNullException("Some helper is null!");
            }
            
            _conferenceFilter = conferenceFilter;
            _controllerHelper = controllerHelper;

            Conferences = _repository.GetAll<Conference>().ToList();
            _conferenceFilter.Conferences = Conferences.OrderBy(e => e.StartDate);
        }
        #endregion

        #region Index
        // GET: Conferences
        public ActionResult Index(string nameFilter, int? tagsFilter)
        {
            _conferenceFilter.FilterByName(ViewData, nameFilter);
            _conferenceFilter.FilterByTags(ViewData, new int[] { tagsFilter ?? 0 }, Tags);

            ViewData["TagsFilter"] = new MultiSelectList(_repository.GetAll<Tag>(), "TagID", "Name");
            ViewData["ResultsCount"] = _controllerHelper.GetResultsCount(_conferenceFilter.Conferences.Count(),true);

            var pageSize = _controllerHelper.GetPageSize(0, PageSize, _conferenceFilter.Conferences.Count());
            PagedConferences =
                _conferenceFilter.Conferences.ToList().GetRange(0, pageSize);
            return View(PagedConferences);
        }

        public JsonResult GetLocations(string locationFilter)
        {
            var cities = _repository.GetAll<City>().ToList();
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
            _conferenceFilter.Conferences = Conferences.OrderBy(e => e.StartDate);
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

            Conference conference = _repository.GetById<Conference>(id);

            if (conference == null)
                return HttpNotFound();

            return View(conference);
        }

        public ActionResult AddEvaluation(int conferenceId, int countOfStars, string comment, string ownerId)
        {
            if (!Request.IsAuthenticated)
                return GetLoginMessage("Log in to add evaluation, please");

            Conference conference = _repository.GetById<Conference>(conferenceId);

            if (conference == null)
                return HttpNotFound();

            if (string.IsNullOrWhiteSpace(comment))
                return GetCommentMessage(conference);

            var eval = _controllerHelper.GetEvaluation(ownerId, comment, countOfStars);
            conference.Evaluation.Add(eval);
            _repository.UpdateAndSubmit(conference);

            return View("Details", conference);
        }

        #endregion

        #region Create
        // GET: Conferences/Create
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
                return GetLoginMessage("Log in to add an event, please");

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

                _repository.InsertAndSubmit(conference);

                if (tags != "null")
                    _controllerHelper.AssignTags(tags, Tags, conference);

                Success("Great job, You added the event!", true);
                return View(conference);
            }
            catch (Exception dbEx)
            {
                Danger("Unable to save changes. Try again, and if the problem persists see your system administrator.", true);
                return View();
            }
        }

        public JsonResult GetSelectedCities(int countryId)
        {
            var country = _repository.GetById<Country>(countryId);
            var selectedCities = country.Cities.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.CityID.ToString()
            });

            return Json(selectedCities, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Manage
        [HttpGet]
        public ActionResult Manage(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Conference conference = _repository.GetById<Conference>(id);


            if (conference == null)
                return HttpNotFound();

            var users = (conference.Speakers == null || conference.Speakers.Count == 0) ? new List<string>() : _controllerHelper.AllUsers.Where(e=>conference.Speakers.Any(l=>l.User.Id == e.Id)).Select(e=>e.Id).ToList();
            ViewData["SpeakersSelector"] = new MultiSelectList(_controllerHelper.AllUsers, "Id", "UserName", new List<string>() { "5fcdb87d-874b-45ed-984f-1cc1742c4f2e" });
            ViewData["Countries"] = new SelectList(Countries, "CountryID", "Name", conference.TargetCountryId);
            ViewData["Cities"] = new SelectList(conference.TargetCountry.Cities, "CityID", "Name", conference.TargetCityId);
            ViewData["TagsSelector"] = new MultiSelectList(Tags, "TagID", "Name", conference.Tags ?? new List<Tag>());
            return View(conference);
        }

        [HttpPost]
        public ActionResult Manage(string speakers, string tags, [Bind(Include = "ConferenceID,Name,StartDate,EndDate,Url,IsPaid,TargetCityId,TargetCountryId")]Conference conference, HttpPostedFileBase imageManage)
        {
            try
            {
                
                var repoConf = _repository.GetById<Conference>(conference.ConferenceID, null);
                var city = _repository.GetById<City>(conference.TargetCityId);
                var country = _repository.GetById<Country>(conference.TargetCountryId);
                _controllerHelper.EditConferenceProperties(conference, repoConf, city, country);
                if (speakers != "null")
                    _controllerHelper.AssignSpeakers(speakers, repoConf);
                if (imageManage != null)
                    _controllerHelper.AssignImage(imageManage, repoConf);

                
                _repository.UpdateAndSubmit(conference);
                conference.ImageId = repoConf.Image != null ? repoConf.Image.ImageID : new int?();

                if (tags != "null")
                    _controllerHelper.AssignTags(tags, Tags, conference);


                ViewData["SpeakersSelector"] = new MultiSelectList(_controllerHelper.AllUsers, "Id", "UserName", repoConf.Speakers ?? new List<Speaker>());
                ViewData["Countries"] = new SelectList(Countries, "CountryID", "Name", conference.TargetCountryId);
                var cities = Cities.Where(e => e.CityID == conference.TargetCityId);
                ViewData["Cities"] = new SelectList(cities, "CityID", "Name", conference.TargetCityId);
                ViewData["TagsSelector"] = new MultiSelectList(Tags, "TagID", "Name", repoConf.Tags ?? new List<Tag>());

                Success("Great job, You change the event!", true);
                return View(conference);
            }
            catch (Exception dbEx)
            {
                Danger("Unable to save changes. Try again, and if the problem persists see your system administrator.", true);
                return View();
            }
        }
        
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.DisposeDataContext();
            }
            base.Dispose(disposing);
        }
    }
}
