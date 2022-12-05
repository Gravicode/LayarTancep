using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class ContactService : ICrud<Contact>
    {
        LayarTancepDB db;

        public ContactService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Contacts.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Contacts.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<Contact> FindByKeyword(string Keyword)
        {
            var data = from x in db.Contacts
                       where x.Subject.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<Contact> GetAllData()
        {
            return db.Contacts.OrderBy(x => x.Id).ToList();
        }

        public Contact GetDataById(object Id)
        {
            return db.Contacts.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Contact data)
        {
            try
            {
                db.Contacts.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Contact data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                /*
                if (sel != null)
                {
                    sel.Nama = data.Nama;
                    sel.Keterangan = data.Keterangan;
                    sel.Tanggal = data.Tanggal;
                    sel.DocumentUrl = data.DocumentUrl;
                    sel.StreamUrl = data.StreamUrl;
                    return true;

                }*/
                return true;
            }
            catch
            {

            }
            return false;
        }

        public long GetLastId()
        {
            return db.Contacts.Max(x => x.Id);
        }
    }

}
/*











 */
