using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Abstract.Helpers;
using WebGrease.Css.Extensions;

namespace ITConferences.WebUI.Helpers
{
    public class FilterHelper : IFilterConferenceHelper
    {
        public IEnumerable<Conference> Conferences { get; set; }

        public void FilterByName(ViewDataDictionary viewData, string nameFilter)
        {
            if (!string.IsNullOrEmpty(nameFilter))
            {
                viewData["NameFilter"] = nameFilter;
                Conferences = Conferences.Where(e => e.Name.ToLower().Contains(nameFilter.ToLower())).ToList();
            }
        }

        public void FilterByLocation(ViewDataDictionary viewData, string locationFilter)
        {
            if (!string.IsNullOrEmpty(locationFilter))
            {
                viewData["LocationFilter"] = locationFilter;
                if (locationFilter.Contains(','))
                {
                    var countryCity = locationFilter.Split(',');
                    var country = countryCity[1].Trim();
                    var city = countryCity[0].Trim();
                    Conferences =
                        Conferences.Where(
                            e =>
                                e.TargetCity.Name.ToLower().Contains(city.ToLower()) &&
                                e.TargetCountry.Name.ToLower().Contains(country.ToLower())).ToList();
                }
                else
                {
                    Conferences =
                        Conferences.Where(
                            e =>
                                e.TargetCity.Name.ToLower().Contains(locationFilter.ToLower()) ||
                                e.TargetCountry.Name.ToLower().Contains(locationFilter.ToLower())).ToList();

                }
            }
        }

        public void FilterByTags(ViewDataDictionary viewData, string[] selectedTagsIds, IEnumerable<Tag> tags)
        {
            if (selectedTagsIds != null && !string.IsNullOrEmpty(selectedTagsIds[0]))
            {
                var selectedTagsInt = new List<int>();
                selectedTagsIds.ForEach(e => selectedTagsInt.Add(int.Parse(e)));
                var selectedTags = tags.Where(e => selectedTagsInt.Contains(e.TagID)).ToList();
                viewData["TagsFilter"] = new MultiSelectList(tags, "TagID", "Name", selectedTags);

                var tempConferences = new List<Conference>();
                foreach (var conference in Conferences)
                {
                    foreach (var selectedTagId in selectedTagsInt)
                    {
                        if (conference.Tags.Any(e => e.TagID == selectedTagId))
                        {
                            if (!tempConferences.Contains(conference))
                            {
                                tempConferences.Add(conference);
                            }
                        }
                    }
                }

                Conferences = tempConferences;
            }
        }

        //TODO: unit tests!
        public void FilterByTime(ViewDataDictionary viewData, DateFilter dateFilter)
        {
            switch (dateFilter)
            {
                case DateFilter.Upcoming:
                    Conferences = Conferences.TakeWhile(e => e.Date >= DateTime.Today);
                    break;

                case DateFilter.Past:
                    Conferences = Conferences.TakeWhile(e => e.Date < DateTime.Today);
                    break;
            }
        }
    }
}
