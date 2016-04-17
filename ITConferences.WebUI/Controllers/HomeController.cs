using System.Web.Mvc;

namespace ITConferences.WebUI.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index(string nameFilter)
        {
            if (!string.IsNullOrEmpty(nameFilter))
            {
                return RedirectToAction("Index", "Conferences", new {nameFilter});
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = @"The website was created for the engineering thesis needs. <br/>
                If you found any bug, please write me: mailto:ewelina.olejnik@outlook.com";

            return View();
        }
    }
}