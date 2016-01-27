using System;
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

        public IEnumerable<Speaker> Speakers { get; private set; }

        #region Ctor
        public SpeakersController(IGenericRepository<Speaker> speakerRepository, IFilterSpeakerHelper speakersFilter)
        {
            if (speakerRepository == null)
            {
                throw new ArgumentNullException("Some repository does not exist!");
            }

            if (speakersFilter == null)
            {
                throw new ArgumentNullException("Filter does not exist!");
            }

            _speakerRepository = speakerRepository;
            _speakersFilter = speakersFilter;
            Speakers = _speakerRepository.GetAll().ToList();
        }
        #endregion

        // GET: Speakers
        public ActionResult Index(string nameFilter)
        {
            _speakersFilter.Speakers = Speakers;
            _speakersFilter.FilterBySpeakerName(ViewData, nameFilter);

            var pageSize = GetPageSize(0);
            var pagedSpeakers =
                _speakersFilter.Speakers.ToList().GetRange(0, pageSize);
            return View(pagedSpeakers);
        }

        public PartialViewResult GetSpeakers(string nameFilter, int? page)
        {
            _speakersFilter.Speakers = Speakers;
            _speakersFilter.FilterBySpeakerName(ViewData, nameFilter);

            var pageId = page ?? 0;
            var pageSize = GetPageSize(pageId);

            if (pageId * PageSize >= _speakersFilter.Speakers.Count())
            {
                return null;
            }

            var pagedSpeakers =
                 _speakersFilter.Speakers.ToList().GetRange(pageId * PageSize, pageSize);
            return PartialView("_SpeakersView", pagedSpeakers);
        }

        private int GetPageSize(int pageId)
        {
            return (PageSize * pageId) + PageSize < _speakersFilter.Speakers.Count()
               ? PageSize
               : Math.Abs(_speakersFilter.Speakers.Count() - (PageSize * pageId));
        }

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

        public ActionResult AddEvaluation(int conferenceId, int countOfStars, string comment)
        {
            if (!Request.IsAuthenticated)
            {
                Danger("Log in to add evaluation, please", true);

                return RedirectToAction("Login", "Account");
            }

            Speaker speaker = _speakerRepository.GetById(conferenceId);

            if (string.IsNullOrWhiteSpace(comment))
            {
                Information("Write a comment to add evaluation, please", true);

                return View("Details", speaker);
            }

            var eval = new Evaluation()
            {
                Comment = comment,
                CountOfStars = countOfStars
            };

            speaker.Evaluations.Add(eval);
            _speakerRepository.UpdateAndSubmit(speaker);

            return View("Details", speaker);
        }


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
