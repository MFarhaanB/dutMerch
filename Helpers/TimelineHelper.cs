using BookStore.Models;
using BookStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BookStore.Helpers
{
    public class TimelineHelper
    {
        public static Int32 GETPROGRESS(Guid Id, Int32 percentage = 0)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                AutoEnquiry autoEnquiry = context.AutoEnquies.Include("Status").FirstOrDefault(x => x.Id == Id);
                switch (autoEnquiry.Status.Key)
                {
                    case "c_enquiry_pending":
                        percentage = 15;
                        break;
                    case "a_awaiting_appointment":
                        percentage = 30;
                        break;
                    case "a_await_repair_acceptance":
                        percentage = 50;
                        break;
                    case "a_await_repair_by_m":
                    case "c_no_repair_checkout":
                        percentage = 70;
                        break;
                    case "c_awaiting_checkout":
                        percentage = 85;
                        break;
                    case "c_auto_checkedout_or_colleced":
                        percentage = 100;
                        break;
                }
            }
            return percentage;
        }

        public static IEnumerable<TimelinesViewModel> GETTIMELINE(Guid Id)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var timeline = context.Timelines.Where(c => c.AutoEnquiryId.Equals(Id)).ToList();

                var grouped = timeline.GroupBy(g => new
                {
                    g.DateTime.Year
                }).Select(c => new
                {
                    GroupKey = c.Key,
                    Events = c.ToList()
                });
                return SortTimelinesData(grouped);
            }
        }

        public static IEnumerable<TimelinesViewModel> SortTimelinesData(IEnumerable<dynamic> grouped)
        {
            var vm = new List<TimelinesViewModel>();
            foreach (var item in grouped)
            {
                vm.Add(new TimelinesViewModel
                {
                    GroupKey = item.GroupKey.Year.ToString(),
                    Timelines = item.Events
                });
            }
            return vm;
        }

        public static void CreateTimelineEvent(Guid Id, String Action, String Description)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Timelines.Add(new Timeline
                {
                    DateTime = DateTime.Now,
                    Action = Action,
                    Description = Description,
                    AutoEnquiryId = Id
                });
                context.SaveChanges();
            }
        }
    }
}