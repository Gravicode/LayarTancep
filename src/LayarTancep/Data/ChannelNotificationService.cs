using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class ChannelNotificationService : ICrud<ChannelNotification>
    {
        LayarTancepDB db;

        public ChannelNotificationService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.ChannelNotifications.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.ChannelNotifications.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<ChannelNotification> FindByKeyword(string Keyword)
        {
            var data = from x in db.ChannelNotifications.Include(c=>c.Channel)
                       where x.Channel.Name.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<ChannelNotification> GetAllData()
        {
            return db.ChannelNotifications.OrderBy(x => x.Id).ToList();
        }

        public ChannelNotification GetDataById(object Id)
        {
            return db.ChannelNotifications.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(ChannelNotification data)
        {
            try
            {
                db.ChannelNotifications.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(ChannelNotification data)
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
            return db.ChannelNotifications.Max(x => x.Id);
        }
    }

}
/*











 */
