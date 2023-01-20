using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class SubscribeService : ICrud<Subscribe>
    {
        LayarTancepDB db;

        public SubscribeService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Subscribes.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Subscribes.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<Subscribe> FindByKeyword(string Keyword)
        {
            var data = from x in db.Subscribes.Include(c=>c.Channel)
                       where x.Channel.Name.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<Subscribe> GetAllData()
        {
            return db.Subscribes.OrderBy(x => x.Id).ToList();
        }
        public List<Subscribe> GetAllData(string UserName)
        {
            return db.Subscribes.Include(c=>c.Channel).Where(x=>x.UserName == UserName).OrderBy(x => x.Id).ToList();
        }
        public Subscribe GetDataById(object Id)
        {
            return db.Subscribes.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Subscribe data)
        {
            try
            {
                db.Subscribes.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Subscribe data)
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
            return db.Subscribes.Max(x => x.Id);
        }
    }

}
/*











 */
