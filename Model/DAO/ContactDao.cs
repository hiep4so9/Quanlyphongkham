using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContactDao
    {
        OnlineShopDBContext db = null;
        public ContactDao()
        {
            db = new OnlineShopDBContext();
        }

        public long Insert(Contact contact)
        {
            db.Contacts.Add(contact);
            db.SaveChanges();
            return contact.ID;
        }
    }
}
