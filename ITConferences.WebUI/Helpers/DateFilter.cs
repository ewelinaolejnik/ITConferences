using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITConferences.WebUI.Helpers
{
    public enum DateFilter
    {
        [Display(Name = "All")]
        All,
        [Display(Name = "Upcoming")]
        Upcoming,
        [Display(Name = "Past")]
        Past
        
    }
}