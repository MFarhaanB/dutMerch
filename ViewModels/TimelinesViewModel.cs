using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.ViewModels
{
    public class TimelinesViewModel
    {
        public string GroupKey { get; set; }
        public List<Timeline> Timelines { get; set; }
    }
}