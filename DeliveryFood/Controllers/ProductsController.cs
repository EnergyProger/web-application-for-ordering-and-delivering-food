using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeliveryFood.Models;

namespace DeliveryFood.Controllers
{
    public class ProductsController : Controller
    {
        private DeliveryEntitiesDb db = new DeliveryEntitiesDb();

        // GET: Products
        public ActionResult Index(int id)
        {
            List<Categories> categories = new List<Categories>();

            using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
            {
                categories = db.Categories.ToList<Categories>();

            }

            ViewData["Category"] = categories;


            string s = "";
            if (Session["Us"] == null)
            {
                s = "";
            }
            else
            {
                s = Session["Us"].ToString();
            }
            //var products = db.Products.Include(p => p.Categories);
            var products = db.Products.Include(p => p.Categories).Where(x => x.CategoryId == id);

            var orderClient = db.Users.Where(u => u.Email == s).FirstOrDefault();

            if (Session["Us"] != null)
            {
                var OrderUser = db.Orders.Include(u => u.Users).Where(u => u.ClientId == orderClient.UserId).FirstOrDefault();
                if (OrderUser != null)
                {
                    ViewBag.SumQuentity = db.Baskets.Include(o => o.Orders).Include(p => p.Products).Where(o => o.OrderId == OrderUser.OrderId).Sum(p => p.Quantity);
                }
                else
                {
                    ViewBag.SumQuentity = null;
                }
                    
            }
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            List<Categories> categories = new List<Categories>();

            using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
            {
                categories = db.Categories.ToList<Categories>();

            }

            string s = "";
            if (Session["Us"] == null)
            {
                s = "";
            }
            else
            {
                s = Session["Us"].ToString();
            }
            

            var orderClient = db.Users.Where(u => u.Email == s).FirstOrDefault();
            if (Session["Us"] != null)
            {
                var OrderUser = db.Orders.Include(u => u.Users).Where(u => u.ClientId == orderClient.UserId).FirstOrDefault();
                if (OrderUser != null)
                {
                    ViewBag.SumQuentity = db.Baskets.Include(o => o.Orders).Include(p => p.Products).Where(o => o.OrderId == OrderUser.OrderId).Sum(p => p.Quantity);
                }
                else
                {
                    ViewBag.SumQuentity = null;
                }

            }
            ViewData["Category"] = categories;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Title");
            return View();
        }

        // POST: Products/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Denomination,CategoryId,Ingredients")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Title", products.CategoryId);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Title", products.CategoryId);
            return View(products);
        }

        // POST: Products/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Denomination,CategoryId,Ingredients")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Title", products.CategoryId);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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
