using System.Net;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;

namespace ITConferences.WebUI.Controllers
{
    public class OrganizersController : BaseController
    {
        #region Ctor

        public OrganizersController(IGenericRepository repository)
            : base(repository)
        {
        }

        #endregion

        // GET: Organizers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var organizer = _repository.GetById<Organizer>(id);

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
                _repository.DisposeDataContext();
            }
            base.Dispose(disposing);
        }
    }
}