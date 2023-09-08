using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Helpers
{
    public class AutoRepairHelper
    {
        public static void SaveEnquiry(ApplicationDbContext context, AutoEnquiry model)
        {
            context.AutoEnquies.Add(model);
            context.SaveChanges();
        }
        public static Status GetStatus(ApplicationDbContext context, int key) => context.Statuses.Find(key);
        public static string GetService(ApplicationDbContext context, Guid key) => context.ServiceTypes.Find(key).Name;
    }
}