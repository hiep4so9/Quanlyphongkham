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
    public class ThuocDao
    {
        OnlineShopDBContext db = null;
        public ThuocDao()
        {
            db = new OnlineShopDBContext();
        }

        public IEnumerable<Thuoc> ListAllPaging(string searchString, int page, int pageSize)
        {
            IOrderedQueryable<Thuoc> query = db.Thuocs;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString))
                    .OrderBy(p => p.Name);
            }
            return query.OrderBy(p => p.Name).ToPagedList(page, pageSize);
        }

        public long Insert(Thuoc thuoc)
        {
            db.Thuocs.Add(thuoc);
            db.SaveChanges();
            return thuoc.id;
        }

        public long Update(Thuoc cl)
        {

            db.Thuocs.AddOrUpdate(cl);
            db.SaveChanges();
            return cl.id;
        }

    }
}
