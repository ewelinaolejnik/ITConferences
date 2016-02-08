using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.WebUI.Abstract.Helpers
{
    public interface IControllerHelper
    {
        string GetResultsCount(int itemsCount, bool empty = false);
        int GetPageSize(int pageId, int pageSize, int itemsCount);
        void AssignSpeaker(string ownerId, string comment, int countOfStars, Speaker speaker, IGenericRepository<Speaker> speakerRepository);
    }
}
