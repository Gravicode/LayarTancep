using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class NotificationService : ICrud<Notification>
    {
        LayarTancepDB db;

        public NotificationService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Notifications.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Notifications.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<Notification> FindByKeyword(string Keyword)
        {
            var data = from x in db.Notifications
                       where x.Message.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<Notification> GetAllData()
        {
            return db.Notifications.OrderBy(x => x.Id).ToList();
        }
        public List<Notification> GetAllData(string Username,int Limit=10)
        {
            return db.Notifications.Where(x=>!x.IsRead).OrderByDescending(x => x.CreatedDate).Take(Limit).ToList();
        }

        public Notification GetDataById(object Id)
        {
            return db.Notifications.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Notification data)
        {
            try
            {
                db.Notifications.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Notification data)
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
        
        public bool UpdateDatas(IEnumerable<Notification> datas)
        {
            try
            {
                foreach (var data in datas)
                {
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                }
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
            return db.Notifications.Max(x => x.Id);
        }
    }

}
/*











 */
