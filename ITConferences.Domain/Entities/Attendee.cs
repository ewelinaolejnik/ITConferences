using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Attendee : IdentityUser
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int AttendeeID { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        //[Display(Name = "Name")]
        //public string Name { get; set; }

        ////[Required]
        ////[Display(Name = "Email")]
        ////[EmailAddress(ErrorMessage = "Invalid email")]
        ////public string Email { get; set; }

        public virtual ICollection<Attendee> Friends { get; set; }
        public virtual ICollection<Conference> FavouriteConferences { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Attendee> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }
}
