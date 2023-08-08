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

        public ActionResult Search(string category, string searchText)
        {
            if(category ==null)
            {
                ViewBag.notify = "Select a category";
                return View("AllProduct", db.Products.ToList());
            }
            else if(searchText==null)
            {
                ViewBag.notify = "Insert something";
                return View("AllProduct", db.Products.ToList());
            }
            else if (category == "name")
            {
                var data = db.Products.SqlQuery("SELECT * FROM Product WHERE Product_Name LIKE '%' + @p0 + '%'", searchText).ToList();
                if (data != null)
                {
                    return View("AllProduct", data);
                }
            }
            else if (category == "brand")
            {
                var data = db.Products.SqlQuery("SELECT * FROM Product WHERE Product_Brand LIKE '%' + @p0 + '%'", searchText).ToList();
                if (data != null)
                {
                    return View("AllProduct", data);
                }
            }
            else if (category == "type")
            {
                var data = db.Products.SqlQuery("SELECT * FROM Product WHERE Product_Type LIKE '%' + @p0 + '%'", searchText).ToList();
                if (data != null)
                {
                    return View("AllProduct", data);
                }
            }
            else if (category == "fabric")
            {
                var data = db.Products.SqlQuery("SELECT * FROM Product WHERE Product_Material LIKE '%' + @p0 + '%'", searchText).ToList();
                if (data != null)
                {
                    return View("AllProduct", data);
                }
            }
            return View("AllProduct");
        }

        

        public ActionResult Filter(string lowPrice, string highPrice)
        {
            int minPrice = Convert.ToInt32(lowPrice);
            int maxPrice = Convert.ToInt32(highPrice);

            var data = db.Products.Where(p => p.Product_Price >= minPrice && p.Product_Price <= maxPrice).ToList();

            /*
            // write string gender in parameter
            if (!string.IsNullOrEmpty(gender))
            {
                filteredProducts = filteredProducts.Where(p => p.Gender == gender);
            }
            */
            return View("AllProduct",data);
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