using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class MedicalExaminationFormController : BaseController
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        // GET: Admin/MedicalExaminationForm

        public ActionResult Create(long? id)
        {
            TempData["apm"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult Create(MedicalExaminationForm model)
        {
            var dao = new AppoimentDao();
            Appointment apm = new Appointment();
            if (TempData.ContainsKey("apm"))
            {
                apm = dao.GetApmById((long)TempData["apm"]);
            }
            if (ModelState.IsValid)
            {
                MedicalExaminationForm form = new MedicalExaminationForm();
                form.PetName = model.PetName;
                form.Age = model.Age;
                form.Weight = model.Weight;
                form.HairColor = model.HairColor;
                form.Species = model.Species;
                form.type = model.type;
                form.id_Appointment = apm.Id;
                form.PetGender = model.PetGender;
                form.ClientId = apm.ClientID;
                form.CreateDate = DateTime.Now;
                db.MedicalExaminationForms.Add(form);
                db.SaveChanges();
                return RedirectToAction("DMEF", new { id = form.id });
            }
            else
            {
                ModelState.AddModelError("", "Error");
                return View(model);
            }
        }


        public ActionResult DMEF(long id)
        {
            var list = db.CacDichVuDaSuDungs.Where(p => p.Id_MEF == id).ToList();
            ViewBag.Id = id;

            var tt = db.CTToaThuocs.Where(p => p.DonThuoc.DeltailsMedicalForm.MedicalExaminationForm.id == id).ToList();
            ViewBag.toathuoc = tt;
            return View(list);
        }

        public ActionResult addService(long id)
        {
            CacDichVuDaSuDung dv = new CacDichVuDaSuDung();
            dv.list = db.DetailsServices.ToList();
            TempData["id"] = id;
            return View(dv);
        }

        [HttpPost]
        public ActionResult addService(CacDichVuDaSuDung model)
        {
            long id = 0;
            if (TempData.ContainsKey("id"))
            {
                try
                {
                    id = (long)TempData["id"];
                    CacDichVuDaSuDung dv = new CacDichVuDaSuDung();
                    dv.Id_DetailsService = model.Id_DetailsService;
                    dv.Id_MEF = id;
                    db.CacDichVuDaSuDungs.Add(dv);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return RedirectToAction("SearchAppointment", "MedicalExaminationForm");
                }
                return RedirectToAction("DMEF", new { id = id });
            }

            return View(model);
        }

        public ActionResult DeleteService(long id, long deleteid)
        {
            var delete = db.CacDichVuDaSuDungs.Where(p => p.Id_DetailsService == id && p.Id_MEF == deleteid).FirstOrDefault();
            return View(delete);
        }

        [HttpPost]
        public ActionResult DeleteService(CacDichVuDaSuDung model)
        {
            var delete = db.CacDichVuDaSuDungs.Where(p => p.Id_DetailsService == model.Id_DetailsService && p.Id_MEF == model.Id_MEF).FirstOrDefault();
            db.CacDichVuDaSuDungs.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("DMEF", new { id = model.Id_MEF });
        }

        [HttpGet]
        public ActionResult SearchAppointment(string searchString)
        {
            var dao = new AppoimentDao();
            var list = dao.SearchAppointment(searchString);
            ViewBag.SearchString = searchString;
            return View(list);

        }

        public ActionResult AddMedicine(long id)
        {
            CTToaThuoc tt = new CTToaThuoc();
            tt.list = db.Thuocs.ToList();
            TempData["idMEF"] = id;
            return View(tt);
        }

        [HttpPost]
        public ActionResult AddMedicine(CTToaThuoc model)
        {
            long id = 0;
            if (TempData.ContainsKey("idMEF"))
            {
                try
                {
                    id = (long)TempData["idMEF"];
                    DonThuoc dt = new DonThuoc();
                    DeltailsMedicalForm deltailsMedicalForm = new DeltailsMedicalForm();
                    deltailsMedicalForm = db.DeltailsMedicalForms.Where(p => p.id_Form == id).FirstOrDefault();
                    if (deltailsMedicalForm == null)
                    {
                        DeltailsMedicalForm dmf = new DeltailsMedicalForm();

                        dmf.id_ill = 1;
                        dmf.id_Form = id;
                        dmf.id_Doctor = 1;
                        db.DeltailsMedicalForms.Add(dmf);
                        db.SaveChanges();
                    }
                    dt = db.DonThuocs.Where(p => p.DeltailsMedicalForm.MedicalExaminationForm.id == id).FirstOrDefault();
                    if (dt != null)
                    {
                        CTToaThuoc ct = new CTToaThuoc();
                        ct.id_Thuoc = model.id_Thuoc;
                        ct.id_DonThuoc = dt.Id;
                        ct.amount = model.amount;
                        ct.Note = model.Note;
                        db.CTToaThuocs.Add(ct);
                        db.SaveChanges();
                    }
                    else
                    {
                        DonThuoc donthuoc = new DonThuoc();
                        donthuoc.id_ill = 1;
                        donthuoc.id_Form = id;
                        db.DonThuocs.Add(donthuoc);
                        db.SaveChanges();
                        CTToaThuoc ct = new CTToaThuoc();
                        ct.id_Thuoc = model.id_Thuoc;
                        ct.id_DonThuoc = donthuoc.Id;
                        ct.amount = model.amount;
                        ct.Note = model.Note;
                        db.CTToaThuocs.Add(ct);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("SearchAppointment", "MedicalExaminationForm");
                }
                return RedirectToAction("DMEF", new { id = id });
            }

            return View(model);
        }

        public ActionResult Index(long? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var DMEF = db.DeltailsMedicalForms.Where(p => p.MedicalExaminationForm.id == id).FirstOrDefault();
            TempData["MEF"] = DMEF.MedicalExaminationForm;
            var toaThuoc = db.CTToaThuocs.Where(p => p.DonThuoc.id_Form == DMEF.id_Form).ToList();
            ViewBag.ToaThuoc = toaThuoc;
            var model = db.CacDichVuDaSuDungs.Where(p => p.Id_MEF == DMEF.MedicalExaminationForm.id).ToList();
            var service = model.Distinct().ToList();
            ViewBag.Service = service;
            ViewBag.Tongtien = TempData["ToIndex"];
            return View(DMEF);
        }

        public ActionResult List(string searchString, int page = 1, int pageSize = 10)
        {
            MedicalExaminationFormDao dao = new MedicalExaminationFormDao();
            var list = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(list);
        }

        public ActionResult Details(long id)
        {
            var details = db.MedicalExaminationForms.Where(n => n.id.Equals(id)).FirstOrDefault();
            return View(details);

        }
    }
}