using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class PageViewService : ICrud<PageView>
    {
        LayarTancepDB db;

        public PageViewService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.PageViews.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.PageViews.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<PageView> FindByKeyword(string Keyword)
        {
            var data = from x in db.PageViews
                       where x.PageName.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<PageView> GetAllData()
        {
            return db.PageViews.OrderBy(x => x.Id).ToList();
        }

        public PageView GetDataById(object Id)
        {
            return db.PageViews.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(PageView data)
        {
            try
            {
                db.PageViews.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(PageView data)
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
            return db.PageViews.Max(x => x.Id);
        }
    }

}
/*











 */
