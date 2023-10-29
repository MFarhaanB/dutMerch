using System.Linq;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Controllers;
using System.Net.Mail;
using System.Net;
using BookStore.Helpers;
using System.Web.Helpers;
using System;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            DatabaseInjector.DBInjector();
            VisitorsHelper.SaveVisitor(Guid.NewGuid().ToString());
            var product = db.Products.ToList();
            return View(product);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}