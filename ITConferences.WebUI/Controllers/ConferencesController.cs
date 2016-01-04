using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;

namespace ITConferences.WebUI.Controllers
{
    public class ConferencesController : BaseController
    {
        private IGenericRepository<Conference> _conferenceRepository;
        private IGenericRepository<Country> _countryRepository;

        public ConferencesController(IGenericRepository<Conference> conferenceRepository, IGenericRepository<Country> countryRepository)
        {
            if (conferenceRepository == null || countryRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }

            _conferenceRepository = conferenceRepository;
            _countryRepository = countryRepository;
        }

        // GET: Conferences
        public ActionResult Index(string filter)
        {
            //var conferences = _conferenceRepository.Include(c => c.TargetCity).Include(c => c.TargetCountry);
            var conferences = _conferenceRepository.GetAll();
            ViewData["Countries"] = new SelectList(_countryRepository.GetAll(), "CountryID", "Name");
            if (!string.IsNullOrEmpty(filter))
            {
                ViewData["Filter"] = filter;
            }
            
            return View(conferences.ToList());
        }

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

        // GET: Conferences/Create
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
            {
                Danger("Log in to add an event, please", true);

                return RedirectToAction("Index", "Home");
            }

            ViewData["Countries"] = new SelectList(_countryRepository.GetAll(), "CountryID", "Name");
            return View();
        }

        // POST: Conferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConferenceID,Name,Date,Url,IsPaid,TargetCityId,TargetCountryId")] Conference conference)
        {
            
            if (ModelState.IsValid)
            {
                _conferenceRepository.InsertAndSubmit(conference);
                return RedirectToAction("Index");
            }
            
            ViewBag.TargetCountryId = new SelectList(_countryRepository.GetAll(), "CountryID", "Name", conference.TargetCountryId);
            return View(conference);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _conferenceRepository.DisposeDataContext();
                _countryRepository.DisposeDataContext();
            }
            base.Dispose(disposing);
        }
    }
}
