using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITConferences.Domain.Entities;
using ITConferences.WebUI.Helpers;

namespace ITConferences.WebUI.Abstract.Helpers
{
    public interface ISortingHelper
    {
        SortingType SortingType { get; set; }

        void Sort(IEnumerable<Conference> conferences);
    }
}
