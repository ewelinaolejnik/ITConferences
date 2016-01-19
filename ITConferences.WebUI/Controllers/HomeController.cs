using System.Web.Mvc;

namespace ITConferences.WebUI.Controllers
{
    [RequireHttps]
    public class HomeController : BaseController
    {
        public ActionResult Index(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                return RedirectToAction("Index", "Conferences", new { searchText = searchText});
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