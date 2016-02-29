using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Entities;
using Microsoft.AspNet.Identity;

namespace ITConferences.WebUI.Models
{
    public class ProfilePictureViewModel
    {
        
        //public ProfilePictureViewModel(IGenericRepository<Attendee> attendeeRepository)
        //{
        //    if (attendeeRepository == null)
        //    {
        //        throw new ArgumentNullException("Attendee model is null");
        //    }
        //    _attendeeRepository = attendeeRepository;
        //}

        //public Image ProfileImage
        //{
        //    get { return _attendeeRepository.GetById(null, HttpContext.Current.User.Identity.GetUserId()).Image; }
        //}
    }
}