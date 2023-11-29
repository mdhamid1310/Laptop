using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaptopSelling.Models;
using PagedList;
using System.Web.UI;


namespace LaptopSelling.Controllers
{
    public class AdminController : Controller
    {

        dbemarketingEntities db=new dbemarketingEntities();
        // GET: Admin
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult login(tbl_admin avm)
        {
            tbl_admin ad=db.tbl_admin.Where(x=>x.ad_username == avm.ad_username && x.ad_password == avm.ad_password).SingleOrDefault();
            if (ad!=null)
            {
                Session["ad_id"] =ad.ad_id.ToString();
                return RedirectToAction("Create");
            }
            else
            {
                ViewBag.error = "inavlid user name or Passwrod";
                
            }
            return View();
        }

        public ActionResult Create()
        {
            if (Session["ad_id"]==null)
            {
                return RedirectToAction("login");

            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(tbl_category cvm,HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if(path.Equals("-1"))
            {
                ViewBag.error = "Image Could not Be uploaded";
            }
            else
            {
                tbl_category cat = new tbl_category();
                cat.cat_name=cvm.cat_name;
                cat.cat_image = path;
                cat.cat_fk_ad = Convert.ToInt32(Session["ad_id"].ToString());
                db.tbl_category.Add(cat);
                db.SaveChanges();


                return RedirectToAction("ViewCategory");

            }
            return View();
        }

        

        public ActionResult ViewCategory(int ? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.OrderByDescending(x => x.cat_id).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);
        }




        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();

            string path = "-1";

            int random = r.Next();

            if(file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if(extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jfif"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                    }
                    catch 
                    {
                        path="-1";
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