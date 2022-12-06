using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class ChannelViewService : ICrud<ChannelView>
    {
        LayarTancepDB db;

        public ChannelViewService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.ChannelViews.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.ChannelViews.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<ChannelView> FindByKeyword(string Keyword)
        {
            var data = from x in db.ChannelViews.Include(c=>c.Channel)
                       where x.Channel.Name.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<ChannelView> GetAllData()
        {
            return db.ChannelViews.OrderBy(x => x.Id).ToList();
        }

        public ChannelView GetDataById(object Id)
        {
            return db.ChannelViews.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(ChannelView data)
        {
            try
            {
                db.ChannelViews.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(ChannelView data)
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
            return db.ChannelViews.Max(x => x.Id);
        }
    }

}
/*











 */
