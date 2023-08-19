using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using VogueLink2.Models;


namespace VogueLink2.Controllers
{
    public class SellerAccessController : Controller
    {

        voguelinkEntities db = new voguelinkEntities();

        // GET: SellerAccess
        public ActionResult Index(int id)
        {
            if(id==1)
            {
                ViewBag.reg = "Registration Successfull";
                ViewBag.not = "Wait for your approval";
            }
            else
            {
                ViewBag.reg = "Registration Unsuccessfull or Error Occured";
                ViewBag.not = "Please try Another time or Contract us";
            }
            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Seller temp)
        {
            var checklogin = db.Sellers.Where(x => x.Seller_Email.Equals(temp.Seller_Email) && x.Seller_Pass.Equals(temp.Seller_Pass)).FirstOrDefault();
            if (checklogin != null && checklogin.Seller_Status=="Approved")
            {
                Session["Seller_Email"] = temp.Seller_Email.ToString();
                Session["Seller_Pass"] = temp.Seller_Pass.ToString();
                Session["Seller_BrandName"] = checklogin.Seller_BrandName.ToString();
                Session["Seller_Id"] = checklogin.Seller_Id;

                return RedirectToAction("ProductViewSeller");
            }
            else if(checklogin != null && checklogin.Seller_Status == "Pending")
            {
                return RedirectToAction("Index", new { id = 1 });
            }
            else
            {
                ViewBag.Notification = "Wrong Email or password";
            }
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Seller cus)
        {
            if (db.Sellers.Any(x => x.Seller_Email == cus.Seller_Email))
            {
                ViewBag.Notification = "Account already existed";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    if (cus.ImageFile != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(cus.ImageFile.FileName);
                        string extension = Path.GetExtension(cus.ImageFile.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        cus.Seller_DP = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        cus.ImageFile.SaveAs(filename);
                    }
                    db.Sellers.Add(cus);
                    db.SaveChanges();
                    
                    var item = db.Sellers.FirstOrDefault(i => i.Seller_Id ==cus.Seller_Id);
                    if (item == null)
                    {
                        return HttpNotFound();
                    }
                    item.Seller_Status = "Pending";
                    db.SaveChanges();
                    ModelState.Clear();

                    return RedirectToAction("Index", new { id = 1 });
                }
                else
                {
                    return RedirectToAction("Index", new { id = 0 });
                }
            }
        }

        public ActionResult SellerDashboard()
        {

            return View();
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product pro)
        {
            if(Session["Seller_Id"]!=null)
            {
                if (ModelState.IsValid)
                {
                    if (pro.ImageFile1 != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile1.FileName);
                        string extension = Path.GetExtension(pro.ImageFile1.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Product_Img1 = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile1.SaveAs(filename);
                    }

                    if (pro.ImageFile2 != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile2.FileName);
                        string extension = Path.GetExtension(pro.ImageFile2.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Product_Img2 = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile2.SaveAs(filename);
                    }
                    if (pro.ImageFile3 != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile3.FileName);
                        string extension = Path.GetExtension(pro.ImageFile3.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Product_Img3 = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile3.SaveAs(filename);
                    }
                    if (pro.ImageFile4 != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile4.FileName);
                        string extension = Path.GetExtension(pro.ImageFile4.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Product_Img4 = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile4.SaveAs(filename);
                    }
                    pro.Seller_Id = (int)Session["Seller_Id"];
                    db.Products.Add(pro);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Product Added Successfully!";
                    ModelState.Clear();
                    return RedirectToAction("ProductViewSeller");
                }
                return RedirectToAction("AddProduct");
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult UpdateProduct(int id)
        {
            if (Session["Seller_Id"] != null)
            {
                var product = db.Products.SingleOrDefault(p => p.Product_Id == id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(Product pro)
        {
            if (Session["Seller_Id"] != null)
            {
                if (ModelState.IsValid)
                {
                    var existingProduct = db.Products.Find(pro.Product_Id);

                    if (existingProduct == null)
                    {
                        return HttpNotFound();
                    }

                   
                    existingProduct.Product_Name = pro.Product_Name;
                    existingProduct.Product_Brand = pro.Product_Brand;
                    existingProduct.Product_Price = pro.Product_Price;
                    existingProduct.Product_Quantity = pro.Product_Quantity;
                    existingProduct.Product_Type = pro.Product_Type;
                    existingProduct.Product_Gender = pro.Product_Gender;
                    existingProduct.Product_Material = pro.Product_Material;
                    existingProduct.Product_Size = pro.Product_Size;
                    existingProduct.Product_Color = pro.Product_Color;

                    if (pro.ImageFile1 != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile1.FileName);
                        string extension = Path.GetExtension(pro.ImageFile1.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Product_Img1 = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile1.SaveAs(filename);
                    }

                    if (pro.ImageFile2 != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile2.FileName);
                        string extension = Path.GetExtension(pro.ImageFile2.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Product_Img2 = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile2.SaveAs(filename);
                    }
                    if (pro.ImageFile3 != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile3.FileName);
                        string extension = Path.GetExtension(pro.ImageFile3.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Product_Img3 = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile3.SaveAs(filename);
                    }
                    if (pro.ImageFile4 != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pro.ImageFile4.FileName);
                        string extension = Path.GetExtension(pro.ImageFile4.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Product_Img4 = "../ProjectImg/" + filename;
                        filename = Path.Combine(Server.MapPath("../ProjectImg/"), filename);
                        pro.ImageFile4.SaveAs(filename);
                    }

                    db.SaveChanges();
                    return RedirectToAction("ProductViewSeller");
                }

                return View("UpdateProduct", new { id = pro.Product_Id });
            }
            return RedirectToAction("Login");
        }

        public ActionResult ProductDelete(int id)
        {
            var pro = db.Products.Find(id);
            if (pro != null)
            {
                db.Products.Remove(pro);
                db.SaveChanges();
                return RedirectToAction("ProductViewSeller");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult ProductViewSeller()
        {
            if (Session["Seller_Id"] != null)
            {

                int temp = (int)Session["Seller_Id"];
                var data = db.Products.Where(p => p.Seller_Id == temp).ToList();
                return View(data);
            }
            return RedirectToAction("Login");
        }


        public ActionResult OrderListDeli()
        {
            if (Session["Seller_Id"] != null)
            {
                DateTime current = DateTime.Now;
                int ids = (int)Session["Seller_Id"];
                var data = db.ProductOrders.Where(o => o.Order_Size=="Done" && o.Seller_Id == ids).ToList();
                int id = (int)Session["Seller_Id"];
                var count = db.ProductOrders.Count(o => o.Seller_Id == id);
                ViewBag.con = count;

                return View(data);
            }
            return RedirectToAction("Login");
        }

        public ActionResult OrderDone(int id)
        {
            if (Session["Seller_Id"] != null)
            {
                var data = db.ProductOrders.Find( id);
                data.Order_Size = "Done";
                db.SaveChanges();
                return RedirectToAction("OrderListDeli");
            }
            return RedirectToAction("Login");
        }

        public ActionResult OrderListtodo()
        {
            if (Session["Seller_Id"] != null)
            {
                DateTime current = DateTime.Now;
                int id = (int)Session["Seller_Id"];
                var data = db.ProductOrders.Where(o => o.Order_Size != "Done" && o.Seller_Id == id).ToList();
                return View(data);
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("AllProduct", "Home");
        }
    }
}