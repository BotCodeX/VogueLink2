﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VogueLink2.Models;

namespace VogueLink2.Controllers
{
    public class UserAccessController : Controller
    {

        voguelinkEntities db = new voguelinkEntities();

        // GET: UserAccess
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Customer cus)
        {
            if(db.Customers.Any(x=>x.Customer_Email == cus.Customer_Email))
            {
                ViewBag.Notification = "Account already existed";
                return View();
            }
            else
            {
                db.Customers.Add(cus);
                db.SaveChanges();
                Session["Customer_FName"] = cus.Customer_FName.ToString();
                Session["Customer_LName"] = cus.Customer_LName.ToString();
                Session["Customer_Email"] = cus.Customer_Email.ToString();
                Session["Customer_Pass"] = cus.Customer_Pass.ToString();
                Session["Customer_Phone"] = cus.Customer_Phone;
                return RedirectToAction("Dashboard");
            }


           
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer cus)
        {
            var checklogin = db.Customers.Where(x => x.Customer_Email.Equals(cus.Customer_Email) && x.Customer_Pass.Equals(cus.Customer_Pass)).FirstOrDefault();
            if(checklogin!=null)
            {
                Session["Customer_Email"] = cus.Customer_Email.ToString();
                Session["Customer_Pass"] = cus.Customer_Pass.ToString();
                Session["Customer_Id"] = checklogin.Customer_Id;
                Session["Customer_FName"] = checklogin.Customer_FName;
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Notification = "Wrong Email or password";
            }
            return View();
        }

        public ActionResult Cart()
        {
            if (Session["Customer_Id"] != null)
            {
                int temp = (int)Session["Customer_Id"];
                var data = db.Carts.Where(p => p.Customer_Id == temp).ToList();
                return View(data);
            }
            return RedirectToAction("Login");
        }
        

        public ActionResult Favourite()
        {
            if (Session["Customer_Id"] != null)
            {
                int temp = (int)Session["Customer_Id"];
                
                    var data = db.Favourates.Where(p => p.Customer_Id == temp).ToList();
                    return View(data);
                
            }
            return RedirectToAction("Login");
        }

        public ActionResult DeleteFav(int id)
        {
            var fav = db.Favourates.Find(id);
            if(fav!=null)
            {
                db.Favourates.Remove(fav);
                db.SaveChanges();
                return RedirectToAction("Favourite");
            }
            else
            {
                return HttpNotFound();
            }
            
        }
    }
}