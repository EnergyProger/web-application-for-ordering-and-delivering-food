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
    public class ClientController : Controller
    {
        DeliveryEntitiesDb db = new DeliveryEntitiesDb();
        Categories categor = new Categories();
        // GET: Person
        public ActionResult Index()
        {
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
            var top = db.Users.Where(u => u.Email == s).FirstOrDefault();
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
            ViewData["Category"] = categor.FetchCategories();
            return View(top);
        }

        //// GET: Person/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Person/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Person/Create
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

        //// GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            //var toper = db.Users.Where(u => u.UserId == id).FirstOrDefault();
            //if(toper.FirstName == null)
            //{
            //    return vie
            //}
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
            var top = db.Users.Where(u => u.Email == s).FirstOrDefault();
            if (Session["Us"] != null)
            {
                var OrderUser = db.Orders.Include(u => u.Users).Where(u => u.ClientId == orderClient.UserId).FirstOrDefault();
                ViewBag.SumQuentity = db.Baskets.Include(o => o.Orders).Include(p => p.Products).Where(o => o.OrderId == OrderUser.OrderId).Sum(p => p.Quantity);

            }
            ViewData["Category"] = categor.FetchCategories();
            return View(top);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users collection)
        {

            var useInfo = db.Users.Where(u => u.UserId == collection.UserId).FirstOrDefault();

            useInfo.FirstName = collection.FirstName;
            useInfo.LastName = collection.LastName;
            useInfo.Birthday = collection.Birthday;

            db.SaveChanges();




            ViewData["Category"] = categor.FetchCategories();

                return RedirectToAction("Index");
            
           
        }

        // GET: Person/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Person/Delete/5
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
