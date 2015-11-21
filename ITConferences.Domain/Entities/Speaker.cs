using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Speaker : Attendee
    {
        public virtual ICollection<Conference> SpokenConferences { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
