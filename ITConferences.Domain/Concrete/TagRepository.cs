using ITConferences.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITConferences.Domain.Entities;

namespace ITConferences.Domain.Concrete
{
    public class TagRepository : ITagRepository
    {
        private ITConferencesContext _context;
        public TagRepository()
        {
            _context = new ITConferencesContext();
        }
        public IEnumerable<Tag> Tags
        {
            get { return _context.Tags; }
        }
    }
}
