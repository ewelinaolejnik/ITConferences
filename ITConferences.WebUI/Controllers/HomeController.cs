using System.Web.Mvc;

namespace ITConferences.WebUI.Controllers
{
    [RequireHttps]
    public class HomeController : BaseController
    {
        public ActionResult Index(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                return RedirectToAction("Index", "Conferences", new { filter = filter});
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