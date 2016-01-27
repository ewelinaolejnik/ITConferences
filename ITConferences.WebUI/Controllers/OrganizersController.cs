using System;
using System.Net;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;

namespace ITConferences.WebUI.Controllers
{
    public class OrganizersController : Controller
    {
        private IGenericRepository<Organizer> _organizerRepository;

        #region Ctor
        public OrganizersController(IGenericRepository<Organizer> organizerRepository)
        {
            if (organizerRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }

            _organizerRepository = organizerRepository;
        }
        #endregion


        // GET: Organizers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Organizer organizer = _organizerRepository.GetById(id);

            if (organizer == null)
            {
                return HttpNotFound();
            }
            return View(organizer);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _organizerRepository.DisposeDataContext();
            }
            base.Dispose(disposing);
        }
    }
}
