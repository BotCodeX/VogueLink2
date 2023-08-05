using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VogueLink2.Models;

namespace VogueLink2.Controllers
{
    public class HomeController : Controller
    {
        voguelinkEntities db = new voguelinkEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Testing()
        {
            ViewBag.Message = "updated.";

            return View();
        }
        public ActionResult AllProduct()
        {
            return View(db.Products.ToList());
        }

        public ActionResult ProductDetails(int id)
        {
            var data = db.Products.FirstOrDefault(s => s.Product_Id == id);
            if(data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }
    }
}