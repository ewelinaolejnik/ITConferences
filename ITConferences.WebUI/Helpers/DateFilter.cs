using System.ComponentModel.DataAnnotations;

namespace ITConferences.WebUI.Helpers
{
    public enum DateFilter
    {
        [Display(Name = "All")] All,
        [Display(Name = "Upcoming")] Upcoming,
        [Display(Name = "Past")] Past
    }
}