using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int errorCode = 0)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false) return RedirectToAction("index", "UserView");
            else if(Session["Id"] != null && (bool)Session["Admin"] == true) return RedirectToAction("index", "Our");
            if (errorCode == 0)
            {
                ViewBag.error = "";   
            }
            else if(errorCode == 1)
            {
                ViewBag.error = "Email or Password is Incorrect!!!";
            }
         
            return View();
        }
        public ActionResult AlreadyLoggedIn(string id)
        {
            using(MyDatabase db = new MyDatabase())
            {
                var user = db.Users.Find(Int16.Parse(id));
                Session["Id"] = user.ID;
                Session["Admin"] = false;
            }
            return RedirectToAction("index", "UserView");
        }
        public ActionResult SignUp(int errorCode = 0)
        {
            if(errorCode == 0)
            {
                ViewBag.error = "";
            }
            else
            {
                ViewBag.error = "Email Already Exist!!!";
            }
            return View();
        }

        [HttpGet]
        public ActionResult verify(string email, string password)
        {
            //verify from admin first
            using(MyDatabase db = new MyDatabase())
            {
                var Admin = db.Admins.Where(ad => ad.email == email && ad.password == password).ToList();
                if(Admin.Count > 0)
                {
                    Session["Id"] = Admin[0].ID;
                    Session["Admin"] = true;

                    return RedirectToAction("index", "Our");
                }
            }
            //Console.WriteLine(email);
            using (MyDatabase db = new MyDatabase())
            {
                var User = db.Users.Where(user => user.Email == email && user.Password == password);

                var UserList = User.ToList();
                if (UserList.Count == 0) return RedirectToAction("index", new {errorCode = 1});
                Session["Id"] = UserList[0].ID;
                Session["Admin"] = false;

                ViewBag.Id = UserList[0].ID;
                ViewBag.Admin = false;
            }
            return View();
        }

        [HttpGet]
        public ActionResult newAccount(string Username, string email, string password, string confirm_password)
        {
            //add new user to database
            //if successfully added, return USerview
            //Console.WriteLine(Username + email + password);
            
            using(MyDatabase db = new MyDatabase())
            {
                var Admin = db.Admins.Where(admin => admin.email == email).ToList();
                if(Admin.Count>0) return RedirectToAction("Signup", new { errorCode = 1 });
                var User = db.Users.Where(user => user.Email == email);
                var UserList = User.ToList();

                if (UserList.Count != 0)
                    return RedirectToAction("Signup", new { errorCode = 1 });
                else
                {
                    User newlyAdd = new User(Username, email, password, "/Content/images/default_image.png");
                    newlyAdd.dateCreated = DateTime.Now;
                    db.Users.Add(newlyAdd);

                    db.SaveChanges();

                    var NewUserId = db.Users.Where(user => user.Email == email);
                    var list = NewUserId.ToList();

                    //Session["id"] = list[0].ID;
                    //Session["Admin"] = false;
                }
            }
            
            return RedirectToAction("index", "Home");

            //else return same view with error "Email already exist"
        }

        public ActionResult Signout()
        {
            Session.Remove("Id");
            Session.Remove("Admin");

            return View();
        }
    }
}