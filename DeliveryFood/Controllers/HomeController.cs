using DeliveryFood.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Data.Entity;


namespace DeliveryFood.Controllers
{
    public class HomeController : Controller
    {
        DeliveryEntitiesDb db = new DeliveryEntitiesDb();

        Categories categor = new Categories();
        

                    
        // GET: Home
        public ActionResult Index()
        {
           
            List<Categories> categories = new List<Categories>();
            //Для вивода даних
            //using(DeliveryEntities db = new DeliveryEntities())
            //{
            //    List<Users> empl = db.Users.ToList<Users>();
            //    return View(empl);
            //}
            using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
            {
                categories = db.Categories.ToList<Categories>();

            }
            string s = "";
            if(Session["Us"] == null)
            {
                s = "";
            }
            else
            {
                s = Session["Us"].ToString();
            }
            
            var ed = db.Users.Where(e => e.Email == s).FirstOrDefault();
            
            ViewData["Users"] = ed;
            ViewData["Category"] = categories;


            var orderClient = db.Users.Where(u => u.Email == s).FirstOrDefault();
           
            if (Session["Us"] != null)
            {
                var OrderUser = db.Orders.Include(u => u.Users).Where(u => u.ClientId == orderClient.UserId).FirstOrDefault();
                if(OrderUser != null)
                {
                    ViewBag.SumQuentity = db.Baskets.Include(o => o.Orders).Include(p => p.Products).Where(o => o.OrderId == OrderUser.OrderId).Sum(p => p.Quantity);
                }
                else
                {
                    ViewBag.SumQuentity = null;
                }
               
            }
           
          
           

           

            

            //ViewData["UserBasket"] = 




            return View();
           
        }

        
      public string Hash(string value)
      {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value)));
      }

        public ActionResult Login()
        {
            FetchCategory();



            return View();
        }

        //Для теста - Авторизація
        //public bool Login(Users user)
        //{
        //    bool result = false;
        //    Users excpeted = new Users();
        //    excpeted.Email = "point@gmail.com";
        //    excpeted.Password = Hash("777777");

        //    if (excpeted.Email == user.Email && excpeted.Password == user.Password)
        //    {
        //        result = true;
        //    }
        //    return result;
        //}

        [HttpPost]
        public ActionResult Login(Users user)
        {
            FetchCategory();

            if (ModelState.IsValid)
            {
                using (var context = new DeliveryEntitiesDb())
                {

                    string s = Hash(user.Password);
                    bool isValid = context.Users.Any(x => x.Email == user.Email && x.Password == s);

                    if (isValid)
                    {

                        Session["Us"] = user.Email;
                       

                        //var ed = db.Users.Where(e => e.Email == user.Email).FirstOrDefault();

                        FormsAuthentication.SetAuthCookie(user.Email, false);

                        HttpCookie cook = new HttpCookie("UserEmail");
                        cook.Value = user.Email;
                        cook.Expires = DateTime.Now.AddDays(2);
                        HttpContext.Response.SetCookie(cook);
                        
                        Session["Cook"] = HttpContext.Request.Cookies.Get("UserEmail");
                        //return RedirectToAction("Index", ed);
                        //return View("Index",ed);
                        return RedirectToAction("Index");


                    }
                    ModelState.AddModelError("", "Неправильний  електронний адрес або пароль");
                    //bool isValid = context.Users.Any(x => x.Email == user.Email && x.Password == user.Password);
                    //if (isValid)
                    //{
                    //    FormsAuthentication.SetAuthCookie(user.Email, false);
                    //    return RedirectToAction("Index", user);
                    //}


                }

            }
           
            return View();


        }
        
        
        public ActionResult Register()
        {
            FetchCategory();
            return View();
        }

        [HttpPost]
        public ActionResult Register(Users user)
        {
            FetchCategory();

            var userTest = db.Users.Where(u => u.Email == user.Email).FirstOrDefault();

            if(userTest != null)
            {
                Session["EmailError"] = "Електроний адрес вже існує";
                
                return View();
            }
           
            

            if (ModelState.IsValid)
            {
                var us = new Users();
                us.Email = user.Email;
                us.Password = Hash(user.Password);
                us.RoleId = 1;
            
                using (var context = new DeliveryEntitiesDb())
                {
                    context.Users.Add(us);
                    context.SaveChanges();
                }
                Session["Message"] = "Аккаунт успішно зареєстровано";
                return RedirectToAction("Login");

            }
            else
            {
                return View();
            }
           
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["Us"] = null;
            return RedirectToAction("Index");
        }


       private void FetchCategory()
        {
            ViewData["Category"] = categor.FetchCategories();
        }

        

    }
}