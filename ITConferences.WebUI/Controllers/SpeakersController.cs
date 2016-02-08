﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;

namespace ITConferences.WebUI.Controllers
{
    public class SpeakersController : BaseController
    {
        private const int PageSize = 6;

        private IGenericRepository<Speaker> _speakerRepository;
        private IFilterSpeakerHelper _speakersFilter;
        private IControllerHelper _controllerHelper;

        public IEnumerable<Speaker> Speakers { get; private set; }

        #region Ctor
        public SpeakersController(IGenericRepository<Speaker> speakerRepository, IFilterSpeakerHelper speakersFilter, IControllerHelper controllerHelper, IGenericRepository<Image> imageRepository) 
            : base(imageRepository)
        {
            if (speakerRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }

            if (speakersFilter == null || controllerHelper == null)
            {
                throw new ArgumentNullException("Filter or controllerHelper does not exist!");
            }

            _speakerRepository = speakerRepository;
            _speakersFilter = speakersFilter;
            Speakers = _speakerRepository.GetAll().ToList();
            _controllerHelper = controllerHelper;
        }
        #endregion

        // GET: Speakers
        public ActionResult Index(string nameFilter)
        {
            _speakersFilter.Speakers = Speakers;
            _speakersFilter.FilterBySpeakerName(ViewData, nameFilter);

            ViewData["ResultsCount"] = _controllerHelper.GetResultsCount(_speakersFilter.Speakers.Count());


            var pageSize = _controllerHelper.GetPageSize(0, PageSize, _speakersFilter.Speakers.Count());
            var pagedSpeakers =
                _speakersFilter.Speakers.ToList().GetRange(0, pageSize);
            return View(pagedSpeakers);
        }

        public PartialViewResult GetSpeakers(string nameFilter, int? page, bool filter = false)
        {
            _speakersFilter.Speakers = Speakers;
            _speakersFilter.FilterBySpeakerName(ViewData, nameFilter);


            if (_speakersFilter.Speakers.Count() == 0)
            {
                return PartialView("_NoResuls");
            }

            ViewData["ResultsCount"] = _controllerHelper.GetResultsCount(_speakersFilter.Speakers.Count(), !filter);

            var pageId = page ?? 0;
            var pageSize = _controllerHelper.GetPageSize(pageId, PageSize, _speakersFilter.Speakers.Count());

            if (pageId * PageSize >= _speakersFilter.Speakers.Count())
            {
                return null;
            }

            var pagedSpeakers =
                 _speakersFilter.Speakers.ToList().GetRange(pageId * PageSize, pageSize);
            return PartialView("_SpeakersView", pagedSpeakers);
        }

        #region Details
        // GET: Speakers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Speaker speaker = _speakerRepository.GetById(id);

            if (speaker == null)
            {
                return HttpNotFound();
            }
            return View(speaker);
        }

        public ActionResult AddEvaluation(int conferenceId, int countOfStars, string comment, string ownerId)
        {
            if (!Request.IsAuthenticated)
                GetLoginMessage();

            Speaker speaker = _speakerRepository.GetById(conferenceId);

            if (string.IsNullOrWhiteSpace(comment))
                GetCommentMessage(speaker);

            _controllerHelper.AssignSpeaker(ownerId, comment, countOfStars, speaker, _speakerRepository);

            return View("Details", speaker);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _speakerRepository.DisposeDataContext();
            }
            base.Dispose(disposing);
        }
    }
}
