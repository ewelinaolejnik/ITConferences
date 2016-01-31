using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;

namespace ITConferences.WebUI.Controllers
{
    [RequireHttps]
    public class HomeController : BaseController
    {
        public ActionResult Index(string nameFilter)
        {
            if (!string.IsNullOrEmpty(nameFilter))
            {
                return RedirectToAction("Index", "Conferences", new { nameFilter = nameFilter });
            }
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}