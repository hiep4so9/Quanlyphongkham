using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        OnlineShopDBContext db = new OnlineShopDBContext();
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            UserDao dao = new UserDao();
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
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var md5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = md5Pas;
                user.CreateTime = DateTime.Now;
                user.GroupID = "MEMBER";
                long id = dao.Insert(user);
                if (id > 0)
                {
                    return RedirectToAction("Index", "User");
                }
            }
            else
            {
                ModelState.AddModelError("", "Them thanh cong");
            }
            return View("Index");
        }

        public ActionResult Edit(int id)
        {
            var Edit = db.Users.Where(n => n.ID.Equals(id)).FirstOrDefault();
            return View(Edit);
        }
        [HttpPost]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {


                UserDao dao = new UserDao();
                var rs = dao.Update(model);
                if (rs > 0)
                {
                    ViewBag.Success = "Success!";
                    return RedirectToAction("Index", "User");
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
            var details = db.Users.Where(n => n.ID.Equals(id)).FirstOrDefault();
            return View(details);
        }
        public ActionResult Delete(int id)
        {
            var Delete = db.Users.Where(n => n.ID.Equals(id)).FirstOrDefault();
            return View(Delete);
        }


        [HttpPost]
        public ActionResult Delete(User model)
        {
            var Delete = db.Users.Where(n => n.ID.Equals(model.ID)).FirstOrDefault();
            db.Users.Remove(Delete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}