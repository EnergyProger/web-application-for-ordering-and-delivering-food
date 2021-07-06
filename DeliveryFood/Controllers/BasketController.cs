using DeliveryFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;


namespace DeliveryFood.Controllers
{
    public class BasketController : Controller
    {
        DeliveryEntitiesDb db = new DeliveryEntitiesDb();
       
        Categories categor = new Categories();
        //Orders order = new Orders();
        // GET: Bakset
      
        public ActionResult Index()
        {
           
            string cc = User.Identity.Name;
            List<Baskets> fb = new List<Baskets>();

            var orderClient = db.Users.Where(u => u.Email == cc).FirstOrDefault();


            var orderUs = db.Orders.Include(u => u.Users).Where(u => u.ClientId == orderClient.UserId).FirstOrDefault();
            if(orderUs == null)
            {

                ViewData["Category"] = categor.FetchCategories();
                ViewData["Bask"] = fb;
                
                return View();
            }

            //var basketUser = db.Baskets.Include(o => o.Orders).Where(p => p.OrderId == orderUs.OrderId).FirstOrDefault();


            ViewData["Category"] = categor.FetchCategories();

            List<Baskets> baskets = new List<Baskets>();



            using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
            {
                baskets = db.Baskets.Include(o => o.Orders).Include(c => c.Products).Where(p => p.OrderId == orderUs.OrderId).ToList();
                

            }

            ViewData["Bask"] = baskets;



            if (Session["Us"] != null)
            {
                var OrderUser = db.Orders.Include(u => u.Users).Where(u => u.ClientId == orderClient.UserId).FirstOrDefault();
                if(OrderUser != null)
                {
                    ViewBag.SumQuentity = db.Baskets.Include(o => o.Orders).Include(p => p.Products).Where(o => o.OrderId == OrderUser.OrderId).Sum(p => p.Quantity);
                }
                else
                {
                    ViewBag.SumQuentity = 0;
                }
                
            }



            return View();
        }

        // GET: Bakset/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //Для теста - добавлення товару до корзини
        //public string AddProduct(Products product)
        //{
        //    return "Товар добавлено до корзини";
        //}
        // GET: Bakset/Create
        //[HttpPost]
        public ActionResult Create(int id)
        {
            string cc = User.Identity.Name;
            var orderClient = db.Users.Where(u => u.Email == cc).FirstOrDefault();
            var baskOrder = db.Orders.Include(u => u.Users).Where(p => p.ClientId == orderClient.UserId).FirstOrDefault();

          

            if (baskOrder != null)
            {

                var checkBask = db.Baskets.Include(p => p.Products).Where(c => c.ProductId == id).Where(v=>v.OrderId == baskOrder.OrderId).FirstOrDefault();
                if (checkBask == null)
                {


                    using (var context = new DeliveryEntitiesDb())
                    {

                        Baskets baskets = new Baskets();
                        // var baskOrderer = db.Orders.Include(u => u.Users).Where(p => p.ClientId == orderClient.UserId).FirstOrDefault();
                        var prodBask = db.Products.Where(p => p.ProductId == id).FirstOrDefault();

                       
                        
                        baskets.OrderId = baskOrder.OrderId;
                        baskets.ProductId = prodBask.ProductId;
                        baskets.Quantity = 1;
                        context.Baskets.Add(baskets);
                        context.SaveChanges();

                        ViewData["Category"] = categor.FetchCategories();
                        //baskOrder.TotalBill = 1 * prodBask.Price;
                        //context.Orders.Add(baskOrder);
                        //context.SaveChanges();
                    }

                    //List<Baskets> basketser = new List<Baskets>();

                    //using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
                    //{
                    //    basketser = db.Baskets.Include(o => o.Orders).Where(p => p.OrderId == baskOrder.OrderId).ToList();

                    //}

                    //ViewData["Bask"] = basketser;
                    List<Baskets> basketser = new List<Baskets>();

                    using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
                    {
                        basketser = db.Baskets.Include(o => o.Orders).Include(c => c.Products).Where(p => p.OrderId == baskOrder.OrderId).ToList();

                    }
                    
                    ViewData["Bask"] = basketser;
                    Session["Order"] = "Товар добавлено до корзини";
                    return RedirectToAction("Index");
                }
                else
                {
                    checkBask.Quantity += 1;
                    
                    db.SaveChanges();
                    //string ccc = User.Identity.Name;


                    //var orderClienter = db.Users.Where(u => u.Email == ccc).FirstOrDefault();


                    var orderUs = db.Orders.Include(u => u.Users).Where(u => u.ClientId == orderClient.UserId).FirstOrDefault();
                    List<Baskets> basketser = new List<Baskets>();

                    using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
                    {
                        basketser = db.Baskets.Include(o => o.Orders).Include(c => c.Products).Where(p => p.OrderId == orderUs.OrderId).ToList();

                    }
                    ViewData["Bask"] = basketser;
                   
                    ViewData["Category"] = categor.FetchCategories();
                    Session["Order"] = "Товар добавлено до корзини";

                    return RedirectToAction("Index");
                }
            }
            else
            {
                using (var context = new DeliveryEntitiesDb())
                {

                    Orders order = new Orders();
                    order.ClientId = orderClient.UserId;
                    context.Orders.Add(order);
                    context.SaveChanges();

                    // var baskOrderer = db.Orders.Include(u => u.Users).Where(p => p.ClientId == orderClient.UserId).FirstOrDefault();
                    var prodBask = db.Products.Where(p => p.ProductId == id).FirstOrDefault();

                    Baskets baskets = new Baskets();
                    baskets.OrderId = order.OrderId;
                    baskets.ProductId = prodBask.ProductId;
                    baskets.Quantity = 1;
                    context.Baskets.Add(baskets);
                    context.SaveChanges();

                    //ViewData["Category"] = categor.FetchCategories();
                    //baskOrder.TotalBill = 1 * prodBask.Price;
                    //context.Orders.Add(baskOrder);
                    //context.SaveChanges();
                }
            }

            
          

            //var checkBask = db.Baskets.Include(p => p.Products).Where(c => c.ProductId == id).FirstOrDefault();
          

            ViewData["Category"] = categor.FetchCategories();

            return View("~/Views/Home/Index.cshtml");
        }

        // POST: Bakset/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Bakset/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    string cc = User.Identity.Name;
        //    var orderClient = db.Users.Where(u => u.Email == cc).FirstOrDefault();
        //    var baskOrder = db.Orders.Include(u => u.Users).Where(p => p.ClientId == orderClient.UserId).FirstOrDefault();
        //    List<Baskets> basketser = new List<Baskets>();

        //    using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
        //    {
        //        basketser = db.Baskets.Include(o => o.Orders).Include(c => c.Products).Where(p => p.OrderId == baskOrder.OrderId).ToList();

        //    }

        //    ViewData["Bask"] = basketser; 
        //    return View("Index");
        //}

        // POST: Bakset/Edit/5
        
        //public ActionResult Edit(int id)
        //{
        //    using(var context = new DeliveryEntitiesDb())
        //    {
        //        var b = context.Baskets.Where(p => p.BasketId == id).FirstOrDefault();
        //        b.BasketId = id;
        //        b.Quantity =
        //        //context.Baskets.Add(b);

        //        context.SaveChanges();

                
        //    }

        //    return RedirectToAction("Index");

        //    //try
        //    //{
        //    //    // TODO: Add update logic here

        //    //    return RedirectToAction("Index");
        //    //}
        //    //catch
        //    //{
        //    //    return View();
        //    //}
        //}

        // GET: Bakset/Delete/5
        public ActionResult Delete(int id)
        {
            //string cc = User.Identity.Name;
            //var orderClient = db.Users.Where(u => u.Email == cc).FirstOrDefault();
            //var baskOrder = db.Orders.Include(u => u.Users).Where(p => p.ClientId == orderClient.UserId).FirstOrDefault();

            var s = db.Baskets.Where(x => x.BasketId == id).FirstOrDefault();
                db.Baskets.Remove(s);
            db.SaveChanges();

            //List<Baskets> basketser = new List<Baskets>();

            //using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
            //{
            //    basketser = db.Baskets.Include(o => o.Orders).Where(p => p.OrderId == baskOrder.OrderId).ToList();

            //}

            //ViewData["Category"] = categor.FetchCategories();

            //List<Baskets> baskets = new List<Baskets>();

            //using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
            //{
            //    baskets = db.Baskets.Include(o => o.Orders).Include(c => c.Products).Where(p => p.OrderId == baskOrder.OrderId).ToList();

            //}

            //ViewData["Bask"] = baskets;

            return RedirectToAction("Index");
        }

        // POST: Bakset/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
