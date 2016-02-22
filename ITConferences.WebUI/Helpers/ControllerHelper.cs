using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;

namespace ITConferences.WebUI.Helpers
{
    public class ControllerHelper : IControllerHelper
    {
        private IGenericRepository<Attendee> _attendeeRepository;
        public ControllerHelper(IGenericRepository<Attendee> attendeeRepository)
        {
            _attendeeRepository = attendeeRepository;
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

        public void AssignSpeaker(string ownerId, string comment, int countOfStars, Speaker speaker,
            IGenericRepository<Speaker> speakerRepository)
        {
            var owner = _attendeeRepository.GetById(null, ownerId);
            var eval = new Evaluation()
            {
                Comment = comment,
                CountOfStars = countOfStars,
                Owner = owner
            };

            speaker.Evaluations.Add(eval);
            speakerRepository.UpdateAndSubmit();
        }
    }
}