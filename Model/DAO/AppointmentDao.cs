using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Model.DAO
{
    public class AppoimentDao
    {
        OnlineShopDBContext db = null;
        public AppoimentDao()
        {
            db = new OnlineShopDBContext();
        }


        public long Insert(Appointment apointment)
        {
            db.Appointments.Add(apointment);
            db.SaveChanges();
            return apointment.Id;
        }

        public long Update(Appointment apointment)
        {
            db.Appointments.AddOrUpdate(apointment);
            db.SaveChanges();
            return apointment.Id;
        }

        public IEnumerable<Appointment> ListAllPaging(string searchString, int page, int pageSize)
        {
            IOrderedQueryable<Appointment> query = db.Appointments;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString) || x.Phone.Contains(searchString))
                    .OrderBy(p => p.BookingDate);
            }
            return query.OrderBy(p => p.BookingDate).ToPagedList(page, pageSize);
        }
        public bool CheckDulicate(DateTime BookingDate, long BookingTime)
        {
            return db.Appointments.Count(p => p.BookingDate == BookingDate && p.BookingTime == BookingTime) > 0;
        }
        public IEnumerable<Appointment> SearchAppointment(string searchString)
        {
            IOrderedQueryable<Appointment> rs = db.Appointments;
            if (!string.IsNullOrEmpty(searchString))
            {
                rs = rs.Where(x => x.Name.Contains(searchString)
                   || x.Phone.Contains(searchString)).OrderByDescending(x => x.DateCreate);
            }
            return rs.OrderByDescending(x => x.DateCreate);
        }

        public Appointment GetApmById(long id)
        {
            var rs = db.Appointments.Where(p => p.Id == id).SingleOrDefault();
            return rs;

        }


    }
}
