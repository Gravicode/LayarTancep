using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class HistoryService : ICrud<History>
    {
        LayarTancepDB db;

        public HistoryService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Historys.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Historys.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<History> FindByKeyword(string Keyword)
        {
            var data = from x in db.Historys
                       where x.UserName.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<History> GetAllData()
        {
            return db.Historys.OrderBy(x => x.Id).ToList();
        }

        public History GetDataById(object Id)
        {
            return db.Historys.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(History data)
        {
            try
            {
                db.Historys.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(History data)
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
            return db.Historys.Max(x => x.Id);
        }
    }

}
/*











 */
