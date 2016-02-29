using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;
using ITConferences.WebUI.Extensions;

namespace ITConferences.WebUI.Helpers
{
    public class ControllerHelper : IControllerHelper
    {
        private IGenericRepository<Attendee> _attendeeRepository;
        public ControllerHelper(IGenericRepository<Attendee> attendeeRepository)
        {
            _attendeeRepository = attendeeRepository;
        }

        public IEnumerable<Attendee> AllUsers
        {
            get { return _attendeeRepository.GetAll(); }
        }

        public string GetResultsCount(int itemsCount, bool empty = false)
        {
            return empty
                ? string.Empty
                : (itemsCount == 1
                    ? itemsCount.ToString() + " result"
                    : itemsCount.ToString() + " results");
        }

        public int GetPageSize(int pageId, int pageSize, int itemsCount)
        {
            return (pageSize * pageId) + pageSize < itemsCount
                ? pageSize
                : Math.Abs(itemsCount) - (pageSize * pageId);
        }

        public Evaluation GetEvaluation(string ownerId, string comment, int countOfStars)
        {
            var owner = _attendeeRepository.GetById(null, ownerId);
            var eval = new Evaluation()
            {
                Comment = comment,
                CountOfStars = countOfStars,
                Owner = owner
            };

            return eval;
        }

        public void AssignImage(HttpPostedFileBase image, Conference conference)
        {
            var img = new Image()
            {
                ImageData = image.InputStream.ToByteArray(),
                ImageMimeType = image.ContentType

            };
            conference.Image = img;
        }

        public void AssignOrganizer(string userId, Conference conference)
        {
            var attendee = _attendeeRepository.GetById(null, userId);
            var organizer = attendee.Organizer ?? new Organizer()
            {
                User = attendee
            };
            

            conference.Organizer = organizer;
        }

        public void AssignTags(string tags, IEnumerable<Tag> tagsList, IGenericRepository<Tag> tagRepository, Conference conference)
        {
            conference.Tags = new List<Tag>();
            var stringTags = tags.Split(',');
            var intTag = stringTags.Select(e => int.Parse(e)).ToList();
            tagsList.Where(e => intTag.Contains(e.TagID)).ToList()
                .ForEach(e => e.Conferences.Add(conference));
            tagRepository.UpdateAndSubmit();
        }
    }
}