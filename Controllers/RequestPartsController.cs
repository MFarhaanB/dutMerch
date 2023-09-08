using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Helpers;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class RequestPartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RequestBooks
        public ActionResult Index()
        {
            ViewBag.Approved = "Approved";
            ViewBag.Pending = "Pending";

            return View(db.PartRequests.ToList());
        }

        // GET: RequestBooks/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartRequest requestBook = db.PartRequests.Find(id);
            if (requestBook == null)
            {
                return HttpNotFound();
            }
            return View(requestBook);
        }

        // GET: RequestBooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PartRequest requestBook)
        {
            requestBook.Date_Requested = DateTime.Now;
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new BaseHelper().CurrentUser(User);
                requestBook.Email = applicationUser.Email;
                db.PartRequests.Add(requestBook);
                db.SaveChanges();

                // send out notification to user
                String body = $"Hi Admin\n\n A part [{requestBook.Name}] has been requested, please login to action.";
                new Email().SendEmail("Hendrix Auto: Part Request", body, "Admin@appdev.com");


                return RedirectToAction("Index");
            }

            return View(requestBook);
        }

        // GET: RequestBooks/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartRequest requestBook = db.PartRequests.Find(id);
            if (requestBook == null)
            {
                return HttpNotFound();
            }
            return View(requestBook);
        }

        // POST: RequestBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartRequest requestBook)
        {
            if (ModelState.IsValid)
            {
                if (requestBook.Admindecision)
                {
                    Products product = new Products
                    {
                        ProductName = requestBook.Name,
                        isActive = true,
                        vModelKey = db.VehicleModels.FirstOrDefault(a=>a.Name.Contains(requestBook.Model)).Id.ToString()??null,
                        //vManufactureKey = db.VehicleModels.FirstOrDefault(a => a.Name.Contains(requestBook.Model)).Id.ToString() ?? null,
                    };
                    db.Products.Add(product);
                    db.SaveChanges();

                    // send out notification to user
                    String body = $"Hi {requestBook.Email}\n\n A part [{requestBook.Name}] has been aproved, please login to action.";
                    new Email().SendEmail("Hendrix Auto: Part Request", body, "Admin@appdev.com");
                }

                db.Entry(requestBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestBook);
        }

        // GET: RequestBooks/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartRequest requestBook = db.PartRequests.Find(id);
            if (requestBook == null)
            {
                return HttpNotFound();
            }
            return View(requestBook);
        }

        // POST: RequestBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PartRequest requestBook = db.PartRequests.Find(id);
            db.PartRequests.Remove(requestBook);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

