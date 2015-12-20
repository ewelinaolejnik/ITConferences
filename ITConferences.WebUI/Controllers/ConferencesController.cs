using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Controllers;

namespace ITConferences.Domain.Controllers
{
    public class ConferencesController : BaseController
    {
        private DataContext db = new DataContext();

        // GET: Conferences
        public ActionResult Index()
        {
            var conferences = db.Conferences.Include(c => c.TargetCity).Include(c => c.TargetCountry);
            return View(conferences.ToList());
        }

        // GET: Conferences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
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
                Danger("Log in to add an event, please",
                true);

                return RedirectToAction("Index", "Home");
            }
            // var countries = new SelectList(db.Countries, "CountryID", "Name");
            ViewData["Countries"] = new SelectList(db.Countries, "CountryID", "Name");
            //ViewBag.TargetCountryId = countries;
            //var selected = countries.SelectedValue;
            //ViewBag.TargetCityId = new SelectList(db.Cities, "CityID", "Name", selected, "DropDownList");
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
                db.Conferences.Add(conference);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.TargetCityId = new SelectList(db.Cities, "CityID", "Name", conference.TargetCityId);
            ViewBag.TargetCountryId = new SelectList(db.Countries, "CountryID", "Name", conference.TargetCountryId);
            return View(conference);
        }

        public JsonResult GetSelectedCities(int countryID)
        {
            var country = db.Countries.Where(e => e.CountryID == countryID).First();
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
