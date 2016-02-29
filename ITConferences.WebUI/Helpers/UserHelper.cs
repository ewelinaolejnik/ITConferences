using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ITConferences.Domain.Abstract;
using ITConferences.Domain.Concrete;
using ITConferences.Domain.Entities;
using Microsoft.AspNet.Identity;
using Ninject;

namespace ITConferences.WebUI.Helpers
{
    public static class UserHelper
    {
       
        public static string GetImage(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity).FindFirst("Image");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : null;
        }
    }
}