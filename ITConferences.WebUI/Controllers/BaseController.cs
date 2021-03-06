﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Models;

namespace ITConferences.WebUI.Controllers
{
    public class BaseController : Controller
    {
        protected IGenericRepository _repository;

        public BaseController(IGenericRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }
            _repository = repository;
        }

        public Dictionary<string, object> ViewDataDictionary { get; set; }

        public FileContentResult GetImage(int? imageId)
        {
            var image = _repository.GetById<Image>(imageId);
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
                ? (List<Alert>) TempData[Alert.TempDataKey]
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