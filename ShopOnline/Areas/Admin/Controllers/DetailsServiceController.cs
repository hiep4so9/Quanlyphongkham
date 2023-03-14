using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class DetailsServiceController : BaseController
    {
        // GET: Admin/DetailsService
        OnlineShopDBContext db = new OnlineShopDBContext();
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            DetailsServiceDao dao = new DetailsServiceDao();
            var list = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            DetailsService detailsService = new DetailsService();
            detailsService.list = db.Services.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(DetailsService detailsService)
        {
            if (ModelState.IsValid)
            {
                Service service = new Service();
                service.id = detailsService.id_services;
                var dao = new DetailsServiceDao();
                long id = dao.Insert(detailsService);
                if (id > 0)
                {
                    return RedirectToAction("Index", "DetailsService");
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
            var Edit = db.DetailsServices.Where(n => n.id.Equals(id)).FirstOrDefault();
            Edit.list = db.Services.ToList();
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
            var details = db.DetailsServices.Where(n => n.id.Equals(id)).FirstOrDefault();
            return View(details);
        }
        public ActionResult Delete(int id)
        {
            var Delete = db.DetailsServices.Where(n => n.id.Equals(id)).FirstOrDefault();
            return View(Delete);
        }


        [HttpPost]
        public ActionResult Delete(User model)
        {
            var Delete = db.DetailsServices.Where(n => n.id.Equals(model.ID)).FirstOrDefault();
            db.DetailsServices.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}