using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;

namespace ITConferences.WebUI.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
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
            ViewBag.Message = @"The website was created for the engineering thesis needs. <br/>
                If you found any bug, please write me: mailto:ewelina.olejnik@outlook.com";

            return View();
        }
    }
}