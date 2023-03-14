using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class ServiceController : BaseController
    {
        // GET: Admin/Service

        OnlineShopDBContext db = new OnlineShopDBContext();
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            ServicesDao dao = new ServicesDao();
            var list = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                
                var dao = new ServicesDao();
                long id = dao.Insert(service);
                if (id > 0)
                {
                    return RedirectToAction("Index", "Service");
                }
            }
            else
            {
                ModelState.AddModelError("", "Error");
            }
            return View("Index");
        }

        public ActionResult Edit(int id)
        {
            var Edit = db.Services.Where(n => n.id.Equals(id)).FirstOrDefault();
            return View(Edit);
        }
        [HttpPost]
        public ActionResult Edit(Service model)
        {
            if (ModelState.IsValid)
            {


                ServicesDao dao = new ServicesDao();
                var rs = dao.Update(model);
                if (rs > 0)
                {
                    ViewBag.Success = "Success!";
                    return RedirectToAction("Index", "Service");
                }
                else
                {
                    ModelState.AddModelError("", "Error");
                }
            }
            return View(model);
        }
        public ActionResult Details(long id)
        {
            var details = db.Services.Where(n => n.id.Equals(id)).FirstOrDefault();
            return View(details);
        }
        public ActionResult Delete(int id)
        {
            var Delete = db.Services.Where(n => n.id.Equals(id)).FirstOrDefault();
            return View(Delete);
        }


        [HttpPost]
        public ActionResult Delete(User model)
        {
            var Delete = db.Services.Where(n => n.id.Equals(model.ID)).FirstOrDefault();
            db.Services.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Assets/Client/images/" + file.FileName));
            return file.FileName;
        }
    }
}
