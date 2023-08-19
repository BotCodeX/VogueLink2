using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
            if (db.Customers.Any(x => x.Customer_Email == cus.Customer_Email))
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
            return RedirectToAction("AllProduct", "Home");
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
            if (checklogin != null)
            {
                Session["Customer_Email"] = cus.Customer_Email.ToString();
                Session["Customer_Pass"] = cus.Customer_Pass.ToString();
                Session["Customer_Id"] = checklogin.Customer_Id;
                Session["Customer_FName"] = checklogin.Customer_FName;
                return RedirectToAction("AllProduct" , "Home");
            }
            else
            {
                ViewBag.Notification = "Wrong Email or password";
            }
            return View();
            /*
            if (!string.IsNullOrEmpty(lemail) && !string.IsNullOrEmpty(lpassword))
            {
                var checklogin = db.Customers.Where(x => x.Customer_Email.Equals(lemail) && x.Customer_Pass.Equals(lpassword)).FirstOrDefault();
                if (checklogin != null)
                {
                    Session["Customer_Email"] = checklogin.Customer_Email.ToString();
                    Session["Customer_Pass"] = checklogin.Customer_Pass.ToString();
                    Session["Customer_Id"] = checklogin.Customer_Id;
                    Session["Customer_FName"] = checklogin.Customer_FName;
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    return View();
                }
            }
            return View("Signup");*/
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["Customer_Id"] != null)
            {
                int temp = (int)Session["Customer_Id"];
                var pro = db.Customers.SingleOrDefault(p => p.Customer_Id == temp);
                if (pro == null)
                {
                    return HttpNotFound();
                }
                return View(pro);
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(Customer pro)
        {
            if (Session["Customer_Id"] != null)
            {
                if (ModelState.IsValid)
                {
                    var existing = db.Customers.Find(pro.Customer_Id);

                    if (existing == null)
                    {
                        return HttpNotFound();
                    }

                    existing.Customer_FName = pro.Customer_FName;
                    existing.Customer_LName = pro.Customer_LName;
                    existing.Customer_Email = pro.Customer_Email;
                    existing.Customer_Pass = pro.Customer_Pass;
                    existing.Customer_Phone = pro.Customer_Phone;
                    existing.Customer_Gender = pro.Customer_Gender;
                    existing.Customer_District = pro.Customer_District;
                    existing.Customer_City = pro.Customer_City;
                    existing.Customer_Area = pro.Customer_Area;
                    existing.Customer_Ship = pro.Customer_Ship;

                    if (pro.ImageFile != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile.FileName);
                        string extension = Path.GetExtension(pro.ImageFile.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Customer_DP = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile.SaveAs(filename);
                    }

                    db.SaveChanges();
                    return RedirectToAction("EditProfile");
                }

                return View("EditProfile");
            }
            return RedirectToAction("Login");
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

        public ActionResult CartDelete(int id)
        {
            var item = db.Carts.Find(id);
            if (item != null)
            {
                db.Carts.Remove(item);
                db.SaveChanges();
                return RedirectToAction("Cart");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult AddFav(int id)
        {
            if (Session["Customer_Id"] != null)
            {
                var newfav = new Favourate
                {
                    Customer_Id = (int)Session["Customer_Id"],
                    Product_Id = id
                };
                return RedirectToAction("ProductDetails", "Home", new { id = id });

            }
            return RedirectToAction("Login");
        }


        public ActionResult AddCart(string size , string amount ,int id)
        {
            if (Session["Customer_Id"] != null)
            {
                if (!string.IsNullOrEmpty(size) && !string.IsNullOrEmpty(amount))
                {
                    var data = db.Products.Where(p => p.Product_Id == id).FirstOrDefault();
                    var newcart = new Cart
                    {
                        Quantity = int.Parse(amount),
                        Price = int.Parse(amount) * data.Product_Price,
                        Customer_Id = (int)Session["Customer_Id"],
                        Promo_Id = 4001,
                        Product_Id = id,
                        Product_Size = size
                    };
                    db.Carts.Add(newcart);
                    db.SaveChanges();
                    return RedirectToAction("Cart");
                }
                return RedirectToAction("ProductDetails", "Home", new { id = id });
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
            if (fav != null)
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

        public ActionResult ApplyVoucher(string voucherCode)
        {
            if (!string.IsNullOrEmpty(voucherCode))
            {
                var check = db.PromoCodes.Where(x => x.Code.Equals(voucherCode)).FirstOrDefault();
                if (check != null)
                {
                    double temp = (double)Session["Payment"];
                    if (temp >= (double)check.Min_Amount)
                    {
                        temp = temp - (double)check.Discount;
                        Session["Payment"] = temp;
                        Session["Discount"] = check.Discount;
                        ViewBag.Message = "Voucher applied successfully!";
                    }
                    else
                    {
                        ViewBag.Message = "Voucher not applicable";
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Voucher";
                }

            }
            else
            {
                ViewBag.Message = "Please enter a voucher code.";
            }
            if (Session["Customer_Id"] != null)
            {
                int temp = (int)Session["Customer_Id"];
                var data = db.Carts.Where(p => p.Customer_Id == temp).ToList();
                return View("Order", data);
            }
            return View();
        }

        public ActionResult Invoice()
        {
            if (Session["Customer_Id"] != null && Session["neworder"] !=null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        public ActionResult Order()
        {
            if (Session["Customer_Id"] != null)
            {
                int temp = (int)Session["Customer_Id"];
                var data = db.Carts.Where(p => p.Customer_Id == temp).ToList();
                double totprice = 0.0;
                foreach (var item in data)
                {
                    totprice = totprice + (double)item.Price;
                }

                Session["Total_Price"] = totprice;
                Session["Payment"] = totprice + 100;
                Session["Quantity"] = db.Carts.Where(p => p.Customer_Id == temp).Sum(p => p.Quantity);
                return View(data);
            }
            return RedirectToAction("Login");
        }


        public ActionResult ConfirmOrder()
        {

            if (Session["Customer_Id"] != null)
            {
                var newOrder = new Order
                {
                    Placed_Date = DateTime.Now,
                    Delivery_Date = DateTime.Now.AddDays(5),
                    Quantity = Convert.ToInt32(Session["Quantity"] ?? 0), 
                    Total_Price = Convert.ToInt32(Session["Payment"] ?? 0), 
                    Product_Id = 5019,
                    Customer_Id = Convert.ToInt32(Session["Customer_Id"])
                };

                db.Orders.Add(newOrder);
                db.SaveChanges();

                Session["DelDate"] = DateTime.Now.AddDays(5);

                int newOrderId = newOrder.Order_Id;
                Session["neworder"] = newOrderId;


                int customerId = (int)Session["Customer_Id"];

                var cartItems = db.Carts.Where(c => c.Customer_Id == customerId).ToList();

               
                foreach (var cartItem in cartItems)
                {
                    
                    var product = db.Products.Find(cartItem.Product_Id);

                    if (product != null)
                    {
                        product.Product_Quantity -= cartItem.Quantity;
                        db.Entry(product).State = EntityState.Modified;
                    }
                }
                    

                foreach (var cartItem in cartItems)
                {
                    //new
                    var seller = db.Products.Find(cartItem.Product_Id);


                    var newPO = new ProductOrder
                    {
                        Order_Price = cartItem.Price,
                        Order_Quantity = cartItem.Quantity,
                        Order_Size = cartItem.Product_Size,
                        Product_Id = cartItem.Product_Id,
                        Order_Id = newOrderId,
                        Seller_Id = seller.Seller_Id
                    };
  
                    db.ProductOrders.Add(newPO);
                }

                db.SaveChanges();

                
                db.Carts.RemoveRange(cartItems);
                db.SaveChanges();

                return RedirectToAction("Invoice");
            }
            return RedirectToAction("Login");
        }

        public ActionResult OrderList()
        {
            if (Session["Customer_Id"] != null )
            {
                int cid = (int)Session["Customer_Id"];
                var order = db.Orders.Where(o => o.Customer_Id == cid).ToList();
                return View(order);
            }
            return RedirectToAction("Login");
        }

        public ActionResult ConOrder()
        {
            if (Session["Customer_Id"] != null)
            {
                int id = (int)Session["Customer_Id"];
                var data = db.Orders.Where(o => o.Customer_Id == id).ToList();
                return View();
            }
            return RedirectToAction("Login");
            
        }
        
    }
}