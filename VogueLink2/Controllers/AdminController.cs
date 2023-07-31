using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VogueLink2.Models;

namespace VogueLink2.Controllers
{
    public class AdminController : Controller
    {

        voguelinkEntities db = new voguelinkEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult Approve()
        {
            return View(db.Sellers.ToList());
        }

        public ActionResult SelllerDetails(int id)
        {
            var seller = db.Sellers.FirstOrDefault(s => s.Seller_Id == id);

            
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        public ActionResult Accept(int id)
        {
            var item = db.Sellers.FirstOrDefault(i => i.Seller_Id == id);
            if(item==null)
            {
                return HttpNotFound();
            }
            item.Seller_Status = "Approved";
            db.SaveChanges();
            return RedirectToAction("SelllerDetails", new { id = item.Seller_Id });
        }

        public ActionResult Reject(int id)
        {
            var item = db.Sellers.FirstOrDefault(i => i.Seller_Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            item.Seller_Status = "Rejected";
            db.SaveChanges();
            return RedirectToAction("SelllerDetails", new { id = item.Seller_Id});
        }

        /*
        [HttpPost]
        public ActionResult Approve(Seller sel)
        {
            /*
            string connectionS = "voguelinkEntities";
            List<Seller> pending = new List<Seller>();
            using (SqlConnection connection = new SqlConnection(connectionS))
            {
                connection.Open();
                string sql = "Select * from Seller where Seller_Status='Pending'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Seller s = new Seller
                            {
                                Seller_Id = (int)reader["Seller_Id"],
                                Seller_BrandName = reader["Seller_BrandName"].ToString()
                            };
                            pending.Add(s);
                        }
                    }
                }
            }
            return View(pending);

            return View();
        }*/
    }
}