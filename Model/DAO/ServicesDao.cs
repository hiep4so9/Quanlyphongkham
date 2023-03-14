using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ServicesDao
    {
        OnlineShopDBContext db = null;
        public ServicesDao()
        {
            db = new OnlineShopDBContext();
        }

        public Service GetServicessById(long? id)
        {
            var service = db.Services.Where(d => d.id == id).FirstOrDefault();
            return service;
        }

        public bool LeaveComment(Feedback feedback)
        {
            db.Feedbacks.Add(feedback);
            
            return db.SaveChanges() > 0;
        }

        public IEnumerable<Service> ListAllPaging(string searchString, int page, int pageSize)
        {
            IOrderedQueryable<Service> query = db.Services;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString))
                    .OrderBy(p => p.Name);
            }
            return query.OrderBy(p => p.Name).ToPagedList(page, pageSize);
        }
        public long Insert(Service service)
        {
            db.Services.Add(service);
            db.SaveChanges();
            return service.id;
        }

        public long Update(Service service)
        {

            db.Services.AddOrUpdate(service);
            db.SaveChanges();
            return service.id;
        }

    }
    
}
