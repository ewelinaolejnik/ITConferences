using System.Collections.Generic;
using System.Web;
using ITConferences.Domain.Entities;

namespace ITConferences.WebUI.Abstract.Helpers
{
    public interface IControllerHelper
    {
        IEnumerable<Attendee> AllUsers { get; }
        string GetResultsCount(int itemsCount, bool empty = false);
        int GetPageSize(int pageId, int pageSize, int itemsCount);
        Evaluation GetEvaluation(string ownerId, string comment, int countOfStars);
        void AssignImage(HttpPostedFileBase image, Conference conference);
        void AssignOrganizer(string userId, Conference conference);
        void AssignTags(string tags, IEnumerable<Tag> tagsList, Conference conference);
        void AssignSpeakers(string speakers, IEnumerable<Speaker> speakersDb, Conference conference);
        void EditConferenceProperties(Conference conference, Conference confToEdit, City city, Country country);
    }
}