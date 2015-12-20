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
    public class SpeakersController : BaseController
    {
        private DataContext db = new DataContext();

        // GET: Speakers
        public ActionResult Index()
        {
            return View(db.Speakers.ToList());
        }

        // GET: Speakers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Speaker speaker = db.Speakers.Find(id);
            if (speaker == null)
            {
                return HttpNotFound();
            }
            return View(speaker);
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
