using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Areas.Admin.Controllers
{
        // GET: Admin/Thuoc
        public class ThuocController : BaseController
        {
            OnlineShopDBContext db = new OnlineShopDBContext();
            // GET: Admin/Doctor
            public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
            {
                ThuocDao dao = new ThuocDao();
                var list = dao.ListAllPaging(searchString, page, pageSize);
                ViewBag.SearchString = searchString;
                return View(list);
            }

            public ActionResult Create()
            {
                Thuoc thuoc = new Thuoc();
                thuoc.list = db.LoaiThuocs.ToList();
                return View(thuoc);
            }
            [HttpPost]
            public ActionResult Create(Thuoc model)
            {
                if (ModelState.IsValid)
                {
                    Thuoc thuoc = new Thuoc();
                    thuoc.Name = model.Name;
                    thuoc.id_LoaiThuoc = model.id_LoaiThuoc;
                    ThuocDao dao = new ThuocDao();
                    var rs = dao.Insert(thuoc);
                    if (rs > 0)
                    {
                        ViewBag.Success = "Success!";
                        return Redirect("/Admin/Thuoc");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error");
                    }
                }
                return View(model);
            }

            public ActionResult Edit(int id)
            {
                var Edit = db.Thuocs.Where(n => n.id.Equals(id)).FirstOrDefault();
                Edit.list = db.LoaiThuocs.ToList();
                return View(Edit);
            }
            [HttpPost]
            public ActionResult Edit(Thuoc model)
            {
                model.list = db.LoaiThuocs.ToList();

                if (ModelState.IsValid)
                {


                    ThuocDao dao = new ThuocDao();
                    var rs = dao.Update(model);
                    if (rs > 0)
                    {
                        ViewBag.Success = "Success!";
                        return RedirectToAction("Index", "Thuoc");
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
                var details = db.Thuocs.Where(n => n.id.Equals(id)).FirstOrDefault();
                return View(details);
            }
            public ActionResult Delete(int id)
            {
                var Delete = db.Thuocs.Where(n => n.id.Equals(id)).FirstOrDefault();
                return View(Delete);
            }

            [HttpPost]
            public ActionResult Delete(Thuoc model)
            {
                var Delete = db.Thuocs.Where(n => n.id.Equals(model.id)).FirstOrDefault();
                db.Thuocs.Remove(Delete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    
}