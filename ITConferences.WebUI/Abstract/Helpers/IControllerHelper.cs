using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ITConferences.WebUI.Abstract.Helpers
{
    public interface IControllerHelper
    {
        string GetResultsCount(int itemsCount, bool empty = false);
        int GetPageSize(int pageId, int pageSize, int itemsCount);
        Evaluation GetEvaluation(string ownerId, string comment, int countOfStars);
        void AssignImage(HttpPostedFileBase image, Conference conference);
        void AssignOrganizer(string userId, Conference conference);
        void AssignTags(string tags, IEnumerable<Tag> tagsList, IGenericRepository<Tag> tagRepository, Conference conference);
    }
}
