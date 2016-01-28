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
using ITConferences.WebUI.Extensions;

namespace ITConferences.WebUI.Controllers
{
    public class ConferencesController : BaseController
    {
        #region Fields
        private const int PageSize = 15;

        private IGenericRepository<Conference> _conferenceRepository;
        private IGenericRepository<Attendee> _attendeeRepository;
        private IGenericRepository<Organizer> _organizerRepository;
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
            IGenericRepository<Tag> tagRepository, IGenericRepository<City> cityRepository, IGenericRepository<Organizer> organizerRepository,
            IFilterConferenceHelper conferenceFilter, IGenericRepository<Image> imageRepository, IGenericRepository<Attendee> attendeeRepository)
        {
            if (conferenceRepository == null || countryRepository == null || tagRepository == null ||
                cityRepository == null || imageRepository == null || attendeeRepository == null || organizerRepository == null)
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
            _attendeeRepository = attendeeRepository;
            _organizerRepository = organizerRepository;
        }
        #endregion

        #region Index
        //TODO: unit tests!
        // GET: Conferences
        public ActionResult Index(string nameFilter, int? tagsFilter)
        {
            ViewData["TagsFilter"] = new MultiSelectList(_tagRepository.GetAll(), "TagID", "Name");
            _conferenceFilter.FilterByName(ViewData, nameFilter);
            _conferenceFilter.FilterByTags(ViewData, new int[] { tagsFilter ?? 0 }, Tags);

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
            _conferenceFilter.Conferences = Conferences.OrderBy(e => e.Date);
            _conferenceFilter.FilterByName(ViewData, nameFilter);
            _conferenceFilter.FilterByLocation(ViewData, locationFilter);
            _conferenceFilter.FilterByTags(ViewData, selectedTagsIds, Tags);
            if (dateFilter != null)
                _conferenceFilter.FilterByTime(ViewData, dateFilter.Value, _conferenceFilter.Conferences);

            var pageId = page ?? 0;
            var pageSize = GetPageSize(pageId);

            if (pageId * PageSize >= _conferenceFilter.Conferences.Count())
            {
                return null;
            }

            var pagedConfs =
                 _conferenceFilter.Conferences.ToList().GetRange(pageId * PageSize, pageSize);
            return PartialView("_ConferencesView", pagedConfs.ToList());
        }

        //TODO: unit tests!
        private int GetPageSize(int pageId)
        {
            return (PageSize * pageId) + PageSize < _conferenceFilter.Conferences.Count()
               ? PageSize
               : Math.Abs(_conferenceFilter.Conferences.Count() - (PageSize * pageId));
        }
        #endregion

        #region Details
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

        public ActionResult AddEvaluation(int conferenceId, int countOfStars, string comment, string ownerId)
        {
            if (!Request.IsAuthenticated)
            {
                Danger("Log in to add evaluation, please", true);

                return RedirectToAction("Login", "Account");
            }

            Conference conference = _conferenceRepository.GetById(conferenceId);

            if (string.IsNullOrWhiteSpace(comment))
            {
                Information("Write a comment to add evaluation, please", true);

                return View("Details", conference);
            }

            var owner = _attendeeRepository.GetById(null, ownerId);
            var eval = new Evaluation()
            {
                Comment = comment,
                CountOfStars = countOfStars,
                Owner = owner
            };
            
            conference.Evaluation.Add(eval);
            _conferenceRepository.UpdateAndSubmit();

            return View("Details", conference);
        }

        #endregion

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
        public ActionResult Create(string tags, Conference conference, HttpPostedFileBase image, string userId)
        {
            ViewData["TagsSelector"] = new MultiSelectList(Tags, "TagID", "Name");
            ViewData["TargetCountryId"] = new SelectList(Countries, "CountryID", "Name");

            try
            {
                if (image != null)
                {
                    var img = new Image()
                    {
                        ImageData = image.InputStream.ToByteArray(),
                        ImageMimeType = image.ContentType

                    };
                    conference.Image = img;
                }

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var attendee = _attendeeRepository.GetById(null, userId);
                    var organizer = new Organizer()
                    {
                        UserId = userId
                    };
                    conference.Organizer = organizer;
                }

                _conferenceRepository.InsertAndSubmit(conference);

                if (tags != "null")
                {
                    var stringTags = tags.Split(',');
                    var intTag = stringTags.Select(e => int.Parse(e)).ToList();
                    var selectedTags = Tags.Where(e => intTag.Contains(e.TagID));
                    Tags.Where(e => intTag.Contains(e.TagID))
                        .ForEach(e => e.Conferences.Add(conference));
                    _tagRepository.UpdateAndSubmit();
                }

                

                Success("Great job, You added the event!", true);
                return View(conference);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                // ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View();
            }
        }

        public FileContentResult GetImage(int? imageId)
        {
            var image = _imageRepository.GetById(imageId); //change for conference
            return File(image.ImageData, image.ImageMimeType);
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
