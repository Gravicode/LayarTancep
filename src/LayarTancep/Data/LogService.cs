using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class LogService : ICrud<Log>
    {
        LayarTancepDB db;

        public LogService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Logs.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Logs.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<Log> FindByKeyword(string Keyword)
        {
            var data = from x in db.Logs
                       where x.Message.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<Log> GetAllData()
        {
            return db.Logs.OrderBy(x => x.Id).ToList();
        }

        public Log GetDataById(object Id)
        {
            return db.Logs.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Log data)
        {
            try
            {
                db.Logs.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Log data)
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
            return db.Logs.Max(x => x.Id);
        }
    }

}
/*











 */
