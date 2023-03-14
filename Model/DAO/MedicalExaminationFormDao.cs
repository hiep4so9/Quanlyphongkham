using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class MedicalExaminationFormDao
    {
        OnlineShopDBContext db = null;
        public MedicalExaminationFormDao()
        {
            db = new OnlineShopDBContext();
        }

        public IEnumerable<MedicalExaminationForm> ListAllPaging(string searchString, int page, int pageSize)
        {
            IOrderedQueryable<MedicalExaminationForm> query = db.MedicalExaminationForms;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.PetName.Contains(searchString) || x.Species.Contains(searchString))
                    .OrderBy(p => p.PetName);
            }
            return query.OrderBy(p => p.PetName).ToPagedList(page, pageSize);
        }
    }
}
