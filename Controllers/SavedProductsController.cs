using BookStore.Models;
using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using BookStore.Helpers;
using System.Configuration;

namespace BookStore.Controllers
{
    public class SavedProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SavedProducts
        public ActionResult Index()
        {

            var wishListsProducts = db.WishListsProducts.Include(w => w.Products).Include(w => w.WishLists);
            return View(wishListsProducts.ToList());
        }

        public ActionResult DisplayWishlist(WishLists wishLists, WishListsProducts wishListsProducts, int? Id, string Wishlisturl)
        {
           var Idc = wishListsProducts.ListId;
            var wlistprod = db.WishListsProducts.Include(y => y.WishLists).Include(x => x.Products).Where(x => x.ListId != wishLists.ListId).ToList();
            TempData["CurrentUrl"] = Request.Url.ToString();
            Wishlisturl = Convert.ToString(TempData["CurrentUrl"]);
            TempData["Wishlisturl"] = Wishlisturl;


            return View(wlistprod);
        }

        [HttpPost]
        public ActionResult ShareWishlist(FormCollection form)
        {

            string Email = form["femail"];

            SendWishlistEmail(Email);

            return View("~/Views/Home/Index.cshtml");
        }

        public void SendWishlistEmail(string Email)
        {
            string subject = "Shared Wishlist";
            string body = "Hi this user:" + " " + User.Identity.Name + " shared their wishlist with you" + " " + TempData["Wishlisturl"];

            try
            {
                new Email().SendEmail(subject, body, Email);
            }
            catch (Exception)
            {

                // throw;
            }
        }


        // GET: SavedProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishListsProducts wishListsProducts = db.WishListsProducts.Find(id);
            if (wishListsProducts == null)
            {
                return HttpNotFound();
            }
            return View(wishListsProducts);
        }

        // GET: SavedProducts/Create
        public ActionResult Create(WishListsProducts wishListsProducts)
        {
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", wishListsProducts.ProductId);
            ViewBag.ListId = new SelectList(db.WishLists, "ListId", "ListName", wishListsProducts.ListId);
            return View();
        }


        // POST: SavedProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create([Bind(Include = "ListedProductsId,ListId,TotalItems,ListUrl,ProductId")] WishListsProducts wishListsProducts, int? id, Products products)
        {
            /* var addedItem = db.WishListsProducts
                  .Single(wishListsProducts => wishListsProducts.ProductId == id);*/

            products.ProductId = Convert.ToInt32(Request.Form["BookId"]);



            if (ModelState.IsValid)
            {

                db.WishListsProducts.Add(wishListsProducts);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (wishListsProducts.ListId != null)
            {

                return RedirectToAction("OurProducts", "Products");

            }


            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", wishListsProducts.ProductId);
            ViewBag.ListId = new SelectList(db.WishLists, "ListId", "ListName", wishListsProducts.ListId);
            return View(wishListsProducts);
        }



        // GET: SavedProducts/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WishListsProducts wishListsProducts = db.WishListsProducts.Find(id);

            if (wishListsProducts == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", wishListsProducts.ProductId);
            ViewBag.ListId = new SelectList(db.WishLists, "ListId", "ListName", wishListsProducts.ListId);
            return View(wishListsProducts);
        }

        // POST: SavedProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListedProductsId,ListId,TotalItems,ListUrl,ProductId")] WishListsProducts wishListsProducts, Products products)
        {

            if (ModelState.IsValid)
            {
                db.Entry(wishListsProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", wishListsProducts.ProductId);
            ViewBag.ListId = new SelectList(db.WishLists, "ListId", "ListName", wishListsProducts.ListId);
            return View(wishListsProducts);
        }

        // GET: SavedProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishListsProducts wishListsProducts = db.WishListsProducts.Find(id);
            if (wishListsProducts == null)
            {
                return HttpNotFound();
            }
            return View(wishListsProducts);
        }

        // POST: SavedProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WishListsProducts wishListsProducts = db.WishListsProducts.Find(id);
            db.WishListsProducts.Remove(wishListsProducts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShareWhishList(Int32 Id)
        {
            var req = Request.Url;
            var baseUrl = ConfigurationManager.AppSettings["SITE_URL"];
            var link = string.Format("{0}/SavedProducts/DisplayWishlist/{1}", baseUrl, Id);
            return Json(link, JsonRequestBehavior.AllowGet);
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