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
        private IGenericRepository<Attendee> _attendeeRepository;
        private IGenericRepository<Image> _imageRepository;
        private IFilterSpeakerHelper _speakersFilter;

        public IEnumerable<Speaker> Speakers { get; private set; }

        #region Ctor
        public SpeakersController(IGenericRepository<Speaker> speakerRepository, IFilterSpeakerHelper speakersFilter, IGenericRepository<Attendee> attendeeRepository, IGenericRepository<Image> imageRepository)
        {
            if (speakerRepository == null || attendeeRepository == null || imageRepository == null)
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
            _attendeeRepository = attendeeRepository;
            _imageRepository = imageRepository;
        }
        #endregion

        // GET: Speakers
        public ActionResult Index(string nameFilter)
        {
            _speakersFilter.Speakers = Speakers;
            _speakersFilter.FilterBySpeakerName(ViewData, nameFilter);

            ViewData["ResultsCount"] = _speakersFilter.Speakers.Count() == 1 
                ? _speakersFilter.Speakers.Count().ToString() + " result" : _speakersFilter.Speakers.Count().ToString() + " results";


            var pageSize = GetPageSize(0);
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

            if (filter)
            {
                ViewData["ResultsCount"] = _speakersFilter.Speakers.Count() == 1 ? _speakersFilter.Speakers.Count().ToString() + " result" : _speakersFilter.Speakers.Count().ToString() + " results";
            }
            else
            {
                ViewData["ResultsCount"] = string.Empty;
            }

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

        public FileContentResult GetImage(int? imageId)
        {
            var image = _imageRepository.GetById(imageId);
            return File(image.ImageData, image.ImageMimeType);
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

        public ActionResult AddEvaluation(int conferenceId, int countOfStars, string comment, string ownerId)
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

            var owner = _attendeeRepository.GetById(null, ownerId);
            var eval = new Evaluation()
            {
                Comment = comment,
                CountOfStars = countOfStars,
                Owner = owner
            };

            speaker.Evaluations.Add(eval);
            _speakerRepository.UpdateAndSubmit();

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
