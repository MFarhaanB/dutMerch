using BookStore.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Helpers
{
    public class BaseHelper
    {
        public ApplicationUser CurrentUser(IPrincipal User)
        {
            String name = User.Identity.Name;
            if (!String.IsNullOrEmpty(name))
            {
                using(ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Users.FirstOrDefault(a => a.UserName == name || a.Email == name);
                }
            }
            else
                return null;
        }

        public ApplicationUser GetUserById(string Id)
        {
            if(String.IsNullOrEmpty(Id))
                return null;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Users.Find(Id);
            }
        }
    }
}