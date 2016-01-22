using System.Collections.Generic;
using System.Web.Mvc;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Helpers;

namespace ITConferences.WebUI.Abstract.Helpers
{
    public interface IFilterConferenceHelper
    {
        IEnumerable<Conference> Conferences { get; set; }

        void FilterByName(ViewDataDictionary viewData, string nameFilter);
        void FilterByLocation(ViewDataDictionary viewData, string locationFilter);
        void FilterByTags(ViewDataDictionary viewData, string[] selectedTagsIds, IEnumerable<Tag> tags);
        void FilterByTime(ViewDataDictionary viewData, DateFilter dateFilter);
    }
}
