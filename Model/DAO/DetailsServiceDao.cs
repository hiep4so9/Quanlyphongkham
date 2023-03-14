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
    public class DetailsServiceDao
    {
        OnlineShopDBContext db = null;
        public DetailsServiceDao()
        {
            db = new OnlineShopDBContext();
        }

        public IEnumerable<DetailsService> ListAllPaging(string searchString, int page, int pageSize)
        {
            IOrderedQueryable<DetailsService> query = db.DetailsServices;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString))
                    .OrderBy(p => p.Name);
            }
            return query.OrderBy(p => p.Name).ToPagedList(page, pageSize);
        }
        public long Insert(DetailsService service)
        {
            db.DetailsServices.Add(service);
            db.SaveChanges();
            return service.id;
        }

        public long Update(DetailsService service)
        {

            db.DetailsServices.AddOrUpdate(service);
            db.SaveChanges();
            return service.id;
        }
    }
}
