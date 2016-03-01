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
        private const int PageSize = 9;

        private IGenericRepository<Speaker> _speakerRepository;
        private IFilterSpeakerHelper _speakersFilter;
        private IControllerHelper _controllerHelper;

        public List<Speaker> PagedSpeakers { get; private set; }

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

        #region Index
        // GET: Speakers
        public ActionResult Index(string nameFilter)
        {
            _speakersFilter.Speakers = Speakers;
            _speakersFilter.FilterBySpeakerName(ViewData, nameFilter);

            ViewData["ResultsCount"] = _controllerHelper.GetResultsCount(_speakersFilter.Speakers.Count());


            var pageSize = _controllerHelper.GetPageSize(0, PageSize, _speakersFilter.Speakers.Count());
            PagedSpeakers =
                _speakersFilter.Speakers.ToList().GetRange(0, pageSize);
            return View(PagedSpeakers);
        }

        public PartialViewResult GetSpeakers(string nameFilter, int? page, bool filter = false)
        {
            _speakersFilter.Speakers = Speakers;
            _speakersFilter.FilterBySpeakerName(ViewData, nameFilter);
            
            if (_speakersFilter.Speakers.Count() == 0)
                return PartialView("_NoResuls");

            if ((page ?? 0) * PageSize >= _speakersFilter.Speakers.Count())
                return null;

            ViewData["ResultsCount"] = _controllerHelper.GetResultsCount(_speakersFilter.Speakers.Count(), !filter);
           
            var pageSize = _controllerHelper.GetPageSize((page ?? 0), PageSize, _speakersFilter.Speakers.Count());
            PagedSpeakers =
                 _speakersFilter.Speakers.ToList().GetRange((page ?? 0) * PageSize, pageSize);
            return PartialView("_SpeakersView", PagedSpeakers);
        }
        #endregion

        #region Details
        // GET: Speakers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Speaker speaker = _speakerRepository.GetById(id);

            if (speaker == null)
                return HttpNotFound();

            return View(speaker);
        }

        public ActionResult AddEvaluation(int speakerId, int countOfStars, string comment, string ownerId)
        {
            if (!Request.IsAuthenticated)
               return GetLoginMessage("Log in to add evaluation, please");

            Speaker speaker = _speakerRepository.GetById(speakerId);

            if (speaker == null)
                return HttpNotFound();

            if (string.IsNullOrWhiteSpace(comment))
               return GetCommentMessage(speaker);

            var eval = _controllerHelper.GetEvaluation(ownerId, comment, countOfStars);
            speaker.Evaluations.Add(eval);
            _speakerRepository.UpdateAndSubmit(speaker);

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
