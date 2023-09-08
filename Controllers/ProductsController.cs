using BookStore.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BookStore.Controllers
{
    //[Authorize]


    public class ProductsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ViewResult Index(string sortOrder, string searchString, ProductCatergory productCatergory)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var students = (from s in db.Products
                            select s);
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.ProductName.Contains(searchString)
                                       || s.ProductPrice.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.ProductName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.ProductPrice);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.ProductPrice);
                    break;
                default:
                    students = students.OrderBy(s => s.ProductName);
                    break;
            }
            var list = students.Include(x => x.ProductCatergory).ToList();
            return View(list);
        }

        // GET: Products 
        public ActionResult OurProducts(string sortOrder, string currentFilter, string searchString, string manufacture, string vModel, string bType, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.manufacture = new SelectList(db.Manufactures, "Id", "Name");
            ViewBag.vModel = new SelectList(db.VehicleModels, "Id", "Name");
            ViewBag.bType = new SelectList(db.VehicleTypes, "Id", "Name");
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!String.IsNullOrEmpty(manufacture) || !String.IsNullOrEmpty(vModel) || !String.IsNullOrEmpty(bType))
            {
                if (!String.IsNullOrEmpty(manufacture))
                {
                    items = items.Where(s => s.vManufactureKey.Contains(manufacture));
                }
                if (!String.IsNullOrEmpty(vModel))
                {
                    items = items.Where(s => s.vModelKey.Contains(vModel));
                }
                if (!String.IsNullOrEmpty(bType))
                {
                    items = items.Where(s => s.vTypeKey.Contains(bType));
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult getVModels(String guid)
        {
            var data = db.VehicleModels.Where(a => a.ManufactureKey == guid).Select(a => new { Value = a.Id, Text = a.Name }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getVTypes(String guid)
        {
            var data = db.VehicleTypes.Where(a => a.VehicleModelKey == guid).Select(a => new { Value = a.Id, Text = a.Name }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Products 
        public ActionResult Education(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products 
        public ActionResult Drama(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products 
        public ActionResult Romance(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products 
        public ActionResult Sci_fi(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Action(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Fantasy(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Comedy(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Religion(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Cooking(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult History(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = db.Products.Include(a => a.ProductCatergory).FirstOrDefault(a => a.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        // GET: Products/Create
        public ActionResult Create(Products product, ProductCatergory productCatergory)
        {
            ViewBag.ListId = new SelectList(db.WishLists, "ListId", "ListName");
            ViewBag.ProductCatergories = new SelectList(db.ProductCategories, "ProductCatergoryId", "CatergoryName");
            ViewBag.Manufactures = new SelectList(db.Manufactures.OrderBy(a => a.Name), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products product, HttpPostedFileBase img_upload, ProductCatergory productCatergory)
        {

            byte[] data;
            int changeCount = 0;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            product.ProductImage = data;
           // product.ProductCreatedAt = DateTime.Now;
          //  product.ProductUpdatedAt = DateTime.Now;
            try
            {
                using (ApplicationDbContext _context = new ApplicationDbContext())
                {
                    _context.Database.CommandTimeout = 200;
                    product.ProductCatergory = null;
                    product.vManufactureKey = product.vManufactureKey;
                    product.vModelKey = product.vModelKey;
                    product.vTypeKey = product.vTypeKey;
                    product.ProductCatergoryId = int.Parse(productCatergory.CatergoryName);
                    _context.Products.Add(product);
                    changeCount = _context.SaveChanges();
                    if (changeCount > 0) return RedirectToAction("Index");
                    else
                    {
                        ViewBag.ProductCatergories = new SelectList(_context.ProductCategories, "ProductCatergoryId", "CatergoryName", product);
                        return View(product);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Products product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Products product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
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
