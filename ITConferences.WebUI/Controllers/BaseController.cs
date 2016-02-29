using ITConferences.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using Microsoft.AspNet.Identity;
using Ninject;

namespace ITConferences.WebUI.Controllers
{
    public class BaseController : Controller
    {
        private IGenericRepository<Image> _imageRepository;
        public Dictionary<string, object> ViewDataDictionary { get; set; }

        public BaseController(IGenericRepository<Image> imageRepository)
        {
            if (imageRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }
            _imageRepository = imageRepository;
        }

        public FileContentResult GetImage(int? imageId)
        {
            var image = _imageRepository.GetById(imageId);
            return File(image.ImageData, image.ImageMimeType);
        }

        protected ActionResult GetLoginMessage(string message)
        {
            Danger(message, true);
            return RedirectToAction("Login", "Account");
        }

        protected ActionResult GetCommentMessage<T>(T item)
        {
            Information("Write a comment to add evaluation, please", true);
            return View("Details", item);
        }

        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }
    }
}