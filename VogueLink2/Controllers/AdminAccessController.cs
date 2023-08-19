using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VogueLink2.Models;
using System.Data.SqlClient;

namespace VogueLink2.Controllers
{
    public class AdminAccessController : Controller
    {

        voguelinkEntities db = new voguelinkEntities();
        // GET: AdminAccess
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(Admin cus)
        {
            var checklogin = db.Admins.Where(x => x.Admin_Email.Equals(cus.Admin_Email) && x.Admin_Pass.Equals(cus.Admin_Pass)).FirstOrDefault();
            if (checklogin != null)
            {
                Session["Admin_Email"] = cus.Admin_Email.ToString();
                Session["Admin_Pass"] = cus.Admin_Pass.ToString();
                Session["Admin_Name"] = checklogin.Admin_FName;
                return RedirectToAction("Approve","Admin");
            }
            else
            {
                ViewBag.Notification = "Wrong Email or password";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("AllProduct", "Home");
        }

    }
}