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
        private IGenericRepository _repository;
        public ControllerHelper(IGenericRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Attendee> AllUsers
        {
            get { return _repository.GetAll<Attendee>(); }
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
            var owner = _repository.GetById<Attendee>(null, ownerId);
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
            var attendee = _repository.GetById<Attendee>(null, userId);
            var organizer = attendee.Organizer ?? new Organizer()
            {
                User = attendee
            };
            

            conference.Organizer = organizer;
        }

        public void AssignTags(string tags, IEnumerable<Tag> tagsList, Conference conference)
        {
            conference.Tags = new List<Tag>();
            var stringTags = tags.Split(',');
            var intTag = stringTags.Select(e => int.Parse(e)).ToList();
            tagsList.Where(e => intTag.Contains(e.TagID)).ToList()
                .ForEach(e => e.Conferences.Add(conference));
            tagsList.ToList().ForEach(e => _repository.UpdateAndSubmit(e));
        }

        public void AssignSpeakers(string speakers, Conference conference)
        {
            conference.Tags = new List<Tag>();
            var stringSpeakers = speakers.Split(',').ToList();
            var attendees = AllUsers.Where(e => stringSpeakers.Any(l => l == e.Id));
            foreach (var attendee in attendees)
            {
                conference.Speakers.Add(new Speaker() { User = attendee });
            }
        }

        public void EditConferenceProperties(Conference conference, Conference confToEdit, City city, Country country)
        {
            confToEdit.Name = conference.Name;
            confToEdit.StartDate = conference.StartDate;
            confToEdit.EndDate = conference.EndDate;
            confToEdit.Url = conference.Url;
            confToEdit.IsPaid = confToEdit.IsPaid;
            confToEdit.TargetCityId = conference.TargetCityId;
            confToEdit.TargetCity = city;
            confToEdit.TargetCountryId = conference.TargetCountryId;
            confToEdit.TargetCountry = country;
        }
    }
}