using Model.DAO;
using Model.EF;
using ShopOnline.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Contact model)
        {
            if (ModelState.IsValid)
            {
                Contact contact = new Contact();
                contact.FullName = model.FullName;
                contact.Note = model.Note;
                contact.Phone = model.Phone;
                contact.Email = model.Email;

                string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/contact.html"));

                content = content.Replace("{{CustomerName}}", model.FullName);
                content = content.Replace("{{Phone}}", model.Phone);
                content = content.Replace("{{Email}}", model.Email);
                content = content.Replace("{{Note}}", model.Note);
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(toEmail, "Pet Clinic", content);

                ContactDao dao = new ContactDao();
                var rs = dao.Insert(contact);
                if (rs > 0)
                {
                    ViewBag.Success = "Success!";
                    return RedirectToAction("ThankContact", "Thanks");
                }
                else
                {
                    ModelState.AddModelError("", "Error");
                }
            }
            return View(model);
        }
    }
}