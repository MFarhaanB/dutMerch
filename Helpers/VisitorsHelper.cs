using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Helpers
{
    public class VisitorsHelper
    {

        public static void SaveVisitor(String SessionId)
        {
            using(ApplicationDbContext _context = new ApplicationDbContext())
            {
                DateTime dateTime = DateTime.Now.Date;
                SystemVisitor visitor = _context.SystemVisitors.FirstOrDefault(c => c.SessionId == SessionId && c.DateVisited.Date == dateTime);
                if (visitor == null)
                {
                    _context.SystemVisitors.Add(new SystemVisitor { SessionId = SessionId, DateVisited = DateTime.Now });
                    _context.SaveChanges();
                }
            }
            
        }
    }
}