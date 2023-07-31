using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using VogueLink2.Models;
using System.Data.Entity.Validation;


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
                return RedirectToAction("AddProduct");
            }
            else if(checklogin.Seller_Status == "Pending")
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

        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product pro)
        {
            if(ModelState.IsValid)
            {
                if(pro.ImageFile1!=null)
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

                db.Products.Add(pro);
                db.SaveChanges();
                TempData["AlertMessage"] = "Product Added Successfully!";
                ModelState.Clear();
                return RedirectToAction("ProductViewSeller");
            }
            return RedirectToAction("AddProduct");
        }

       
        public ActionResult ProductViewSeller()
        {
            return View(db.Products.ToList());
        }
    }
}