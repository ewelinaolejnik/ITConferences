﻿using System.Collections.Generic;
using System.Web.Mvc;
using ITConferences.Domain.Entities;

namespace ITConferences.WebUI.Abstract.Helpers
{
    public interface IFilterSpeakerHelper
    {
        IEnumerable<Speaker> Speakers { get; set; }


        void FilterBySpeakerName(ViewDataDictionary viewData, string nameFilter);
    }
}