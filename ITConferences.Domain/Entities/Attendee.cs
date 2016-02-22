using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Attendee : IdentityUser
    {
        public virtual Image Image { get; set; }

        [ForeignKey("Image")]
        public int? ImageId { get; set; }

        public virtual Organizer Organizer { get; set; }
        public virtual Speaker Speaker { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Attendee> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

    }
}
