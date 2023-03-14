using Model.DAO;
using Model.EF;
using ShopOnline.Common;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ShopOnline.Controllers
{
    public class AppointmentController : Controller
    {
        OnlineShopDBContext context = new OnlineShopDBContext();
        // GET: Appointment
        public ActionResult Index(long? id)
        {
            var session = (ShopOnline.Common.UserLogin)Session[ShopOnline.Common.ConstantsCommon.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("login", "user");
            }
            Appointment apointment = new Appointment();
            apointment.list = context.Services.ToList();
            if(id.HasValue)
            {
                var dao = new ServicesDao().GetServicessById(id);
                Session[ConstantsCommon.SERVICES_SESSION] = null;
                ViewBag.id = dao;
                var ServicesId = new UserServices();
                ServicesId.ID = dao.id;
                Session.Add(ConstantsCommon.SERVICES_SESSION, ServicesId);
            }
            return View(apointment);
            
        }

        [HttpPost]
        public ActionResult Index(Appointment model)
        {
            var session = (ShopOnline.Common.UserLogin)Session[ShopOnline.Common.ConstantsCommon.USER_SESSION];
            var ServicesId = (ShopOnline.Common.UserServices)Session[ConstantsCommon.SERVICES_SESSION];
            if (session == null)
            {
                return RedirectToAction("login", "user");
            }
            var client = new UserDao().getClientById(session.ID);
            model.list = context.Services.ToList();
            if (ModelState.IsValid)
            {
                Appointment appointmentModel = new Appointment();
                appointmentModel.Name = model.Name;
                appointmentModel.Email = model.Email;
                appointmentModel.Phone = model.Phone;
                appointmentModel.Note = model.Note;
                appointmentModel.status = -1;
                AppoimentDao dao = new AppoimentDao();

                var dt = model.BookingDate;
                var dtn = DateTime.Today;
                var res = DateTime.Compare(dt, dtn);
                if (res < 0)
                {
                    ModelState.AddModelError("", "Ngày đặt phải lớn hơn hoặc là ngày hiện tại");
                    return View("Index", model);
                }


                DateTime dateTime = DateTime.Today;
                TimeSpan ca1 = new TimeSpan(7, 30, 0);
                TimeSpan ca2 = new TimeSpan(9, 30, 0);
                TimeSpan ca3 = new TimeSpan(13, 30, 0);
                TimeSpan ca4 = new TimeSpan(15, 30, 0);

                DateTime c1 = dateTime.Add(ca1);
                DateTime c2 = dateTime.Add(ca2);
                DateTime c3 = dateTime.Add(ca3);
                DateTime c4 = dateTime.Add(ca4);
                DateTime add = model.BookingDate.Add(ShiftToTime.shiftToTime1((long)model.BookingTime));

                var dtnow = DateTime.Now;
                if (add <= c1 && dtnow >= c1)
                {
                    ModelState.AddModelError("", "Đã qua thời gian ca 1");
                    return View("Index",model);
                }
                else if (add <= c2 && dtnow >= c2)
                {
                    ModelState.AddModelError("", "Đã qua thời gian ca 2");
                    return View("Index",model);
                }
                else if (add <= c3 && dtnow >= c3)
                {
                    ModelState.AddModelError("", "Đã qua thời gian ca 3");
                    return View("Index",model);
                }
                else if (add <= c4 && dtnow >= c4)
                {
                    ModelState.AddModelError("", "Đã qua thời gian ca 4");
                    return View("Index",model);
                }

                if (dao.CheckDulicate(model.BookingDate, (long)model.BookingTime))
                {
                    ModelState.AddModelError("", "Giờ đặt lịch đã trùng");
                    return View("Index",model);
                }

                appointmentModel.BookingDate = model.BookingDate;
                appointmentModel.BookingTime = model.BookingTime;
                appointmentModel.DateCreate = DateTime.Now;
                if (ServicesId != null)
                {
                    appointmentModel.ServicesId = ServicesId.ID;
                }
                else
                {
                    appointmentModel.ServicesId = model.ServicesId;
                }
                appointmentModel.ClientID = client.id;
                model.list = context.Services.ToList();

                string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html"));

                var Service = "";
                if (ServicesId != null)
                {
                    Service = new ServicesDao().GetServicessById(ServicesId.ID).Name;
                }
                else
                {
                    Service = new ServicesDao().GetServicessById(model.ServicesId).Name;
                }
                content = content.Replace("{{CustomerName}}", model.Name);
                content = content.Replace("{{Phone}}", model.Phone);
                content = content.Replace("{{Email}}", model.Email);
                content = content.Replace("{{BookingDate}}", model.BookingDate.ToString("dd/MM/yyyy"));
                content = content.Replace("{{BookingTime}}", ShiftToTime.shiftToTime(model.BookingTime));
                content = content.Replace("{{Service}}", Service);
                content = content.Replace("{{Note}}", model.Note);
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(model.Email, "Pet Clinic", content);
                new MailHelper().SendMail(toEmail, "Pet Clinic", content);


                var rs = dao.Insert(appointmentModel);
                if (rs > 0)
                {

                    ViewBag.Success = "Success!";
                    Session[ConstantsCommon.SERVICES_SESSION] = null;
                    return RedirectToAction("Index", "Thanks");
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
            var session = (ShopOnline.Common.UserLogin)Session[ShopOnline.Common.ConstantsCommon.USER_SESSION];           
            if (session == null)
            {
                return RedirectToAction("login", "user");
            }
           
            var appointment = context.Appointments.Where(m=>m.Id == id).FirstOrDefault();
            var deltailsMedicalForms = context.DeltailsMedicalForms.Where(m => m.MedicalExaminationForm.id_Appointment == id).FirstOrDefault();
            if (deltailsMedicalForms == null)
            {
                return View("DetailsComing", appointment);
            }
            else
            {
                TempData["appointment"] = appointment;
                return View(deltailsMedicalForms);
            }    
            
           
        }


        [HttpPost]
        public ActionResult leaveComment(CommentModel model)
        {
            //string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            var session = (ShopOnline.Common.UserLogin)Session["USER_SESSION"];
            var appointment = new Appointment();
            if (session == null)
            {
                return RedirectToAction("login", "user");
            }
            if(TempData.ContainsKey("appointment"))
            {
                appointment = TempData["appointment"] as Appointment;
            }
            var feedback = new Feedback();
            feedback.text = model.text;
            feedback.EntityID = model.EntityID;
            feedback.User_id = session.ID;
            feedback.CreateTime = DateTime.Now;
            feedback.Serviced_Id = appointment.ServicesId;
            
            appointment.status = 1; // đã đánh giá **** nhớ sửa thành 1 để -1 để chạy code thử
            var appointmentDao = new AppoimentDao().Update(appointment);
            var servicesDao = new ServicesDao().LeaveComment(feedback);
            if(servicesDao)
            {

                return Redirect("/Appointment/Details/"+ appointment.Id);
            }
            else
            {
                return HttpNotFound();
            }
        }




    }
}