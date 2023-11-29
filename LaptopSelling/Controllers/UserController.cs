using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaptopSelling.Models;
using PagedList;
using System.Web.UI;
using System.IO;

namespace LaptopSelling.Controllers
{
    public class UserController : Controller
    {
        dbemarketingEntities db = new dbemarketingEntities();
        // GET: User
        public ActionResult Index(int ?page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(tbl_user uvm, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                tbl_user u = new tbl_user();
                u.u_name = uvm.u_name;
                u.u_email = uvm.u_email;
                u.u_password = uvm.u_password;
                u.u_image = path;
                u.u_contact = uvm.u_contact;
                db.tbl_user.Add(u);
                db.SaveChanges();
                return RedirectToAction("Login");

            }

            return View();
        }


        public ActionResult login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult login(tbl_user avm)
        {
            tbl_user ad = db.tbl_user.Where(x => x.u_email == avm.u_email && x.u_password == avm.u_password).SingleOrDefault();
            if (ad != null)
            {

                Session["u_id"] = ad.u_id.ToString();
                return RedirectToAction("CreateAd");

            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }

            return View();
        }


        [HttpGet]
        public ActionResult CreateAd()
        {
            List<tbl_category> li = db.tbl_category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");

            return View();
        }

        [HttpPost]
        public ActionResult CreateAd(tbl_product pvm, HttpPostedFileBase imgfile)
        {
            List<tbl_category> li = db.tbl_category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");


            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                tbl_product p = new tbl_product();
                p.pro_name = pvm.pro_name;
                p.pro_price = pvm.pro_price;
                p.pro_image = path;
                p.pro_fk_cat = pvm.pro_fk_cat;
                p.pro_des = pvm.pro_des;
                p.pro_fk_user = Convert.ToInt32(Session["u_id"].ToString());
                db.tbl_product.Add(p);
                db.SaveChanges();
                Response.Redirect("index");

            }

            return View();
        }



























        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();

            string path = "-1";

            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jfif"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                    }
                    catch
                    {
                        path = "-1";
                    }

                }
                else
                {
                    Response.Write("<script> alert ('only jpg,jpeg or png formats are accepted...');</script>");
                }
            }
            else
            {
                Response.Write("<script> alert ('Please Select File');</script>");

            }
            return path;

        }
    }
}