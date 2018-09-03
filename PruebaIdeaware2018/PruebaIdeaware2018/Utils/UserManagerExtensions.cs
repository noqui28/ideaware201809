using Microsoft.AspNet.Identity;
using PruebaIdeaware2018.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Utils
{
    public static class UserManagerExtensions
    {
        public static ApplicationUser FindByCedulaAsync(this UserManager<ApplicationUser> um, int cedula)
        {
            return um?.Users?.SingleOrDefault(x => x.Cedula == cedula);
        }
    }
}