using System;
using System.Collections;
using System.Collections.Generic;
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
        private IGenericRepository<City> _cityRepository;
        public IEnumerable<Conference> Conferences; 

        public ConferencesController(IGenericRepository<Conference> conferenceRepository, IGenericRepository<Country> countryRepository, IGenericRepository<City> cityRepository)
        {
            if (conferenceRepository == null || countryRepository == null || cityRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }

            _conferenceRepository = conferenceRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            Conferences = _conferenceRepository.GetAll().ToList();
        }

        // GET: Conferences
        public ActionResult Index(string nameFilter, string locationFilter)
        {
            //var conferences = _conferenceRepository.Include(c => c.TargetCity).Include(c => c.TargetCountry);
            //Conferences = _conferenceRepository.GetAll().ToList();
            ViewData["Countries"] = new SelectList(_countryRepository.GetAll(), "CountryID", "Name");

            if (!string.IsNullOrEmpty(nameFilter))
            {
                ViewData["NameFilter"] = nameFilter;
                Conferences = Conferences.Where(e => e.Name.ToLower().Contains(nameFilter.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(locationFilter))
            {
                ViewData["LocationFilter"] = locationFilter;
                if (locationFilter.Contains(','))
                {
                    var countryCity = locationFilter.Split(',');
                    var country = countryCity[1].Trim();
                    var city = countryCity[0].Trim();
                    Conferences =
                        Conferences.Where(
                            e =>
                                e.TargetCity.Name.ToLower().Contains(city.ToLower()) ||
                                e.TargetCountry.Name.ToLower().Contains(country.ToLower())).ToList();
                }
                else
                {
                    Conferences =
                        Conferences.Where(
                            e =>
                                e.TargetCity.Name.ToLower().Contains(locationFilter.ToLower()) ||
                                e.TargetCountry.Name.ToLower().Contains(locationFilter.ToLower())).ToList();
                }
            }
            
            return View(Conferences);
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

                return RedirectToAction("Login", "Account");
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

        public JsonResult GetLocations(string locationFilter)
        {
            var filteredConferences = Conferences.Where(
                e =>
                    e.TargetCity.Name.ToLower().StartsWith(locationFilter.ToLower()) ||
                    e.TargetCountry.Name.ToLower().StartsWith(locationFilter.ToLower())).ToList();

            List<string> result = new List<string>();

            filteredConferences.ForEach(e=>result.Add(e.TargetCity.Name + ", " + e.TargetCountry.Name));

            return Json(result, JsonRequestBehavior.AllowGet);
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
