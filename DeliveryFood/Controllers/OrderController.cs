using DeliveryFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace DeliveryFood.Controllers
{
    public class OrderController : Controller
    {
        private DeliveryEntitiesDb db = new DeliveryEntitiesDb();
        // GET: Order
        public ActionResult Index()
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
            //var products = db.Products.Include(p => p.Categories).Where(x => x.CategoryId == id);

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
            List<Baskets> baskets = new List<Baskets>();


            var orderUs = db.Orders.Include(u => u.Users).Where(u => u.ClientId == orderClient.UserId).FirstOrDefault();
            using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
            {
                baskets = db.Baskets.Include(o => o.Orders).Include(c => c.Products).Where(p => p.OrderId == orderUs.OrderId).ToList();


            }

            ViewData["Bask"] = baskets;
            var userInfo = db.Orders.Include(u => u.Users).Where(x => x.ClientId == orderClient.UserId).FirstOrDefault();
            return View(userInfo);
        }

        [HttpPost]
        public ActionResult Create()
        {
            Session["Info"] = "Замовлення прийнято";
            return Redirect("~/Client/Index");
        }

        //public string Create(Orders orders)
        //{
           
        //    return "Замовлення прийнято";
        //}

    }
}