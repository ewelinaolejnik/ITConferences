using ITConferences.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Abstract
{
    public interface ITagRepository
    {
        IEnumerable<Tag> Tags { get; }
    }
}
