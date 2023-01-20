using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class NewsLetterSubscriberService : ICrud<NewsLetterSubscriber>
    {
        LayarTancepDB db;

        public NewsLetterSubscriberService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.NewsLetterSubscribers.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.NewsLetterSubscribers.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<NewsLetterSubscriber> FindByKeyword(string Keyword)
        {
            var data = from x in db.NewsLetterSubscribers
                       where x.Email.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<NewsLetterSubscriber> GetAllData()
        {
            return db.NewsLetterSubscribers.OrderBy(x => x.Id).ToList();
        }

        public NewsLetterSubscriber GetDataById(object Id)
        {
            return db.NewsLetterSubscribers.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(NewsLetterSubscriber data)
        {
            try
            {
                db.NewsLetterSubscribers.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(NewsLetterSubscriber data)
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

        public bool IsExist(string Email)
        {
            return db.NewsLetterSubscribers.Any(x => x.Email.ToLower() == Email.ToLower());
        }

        public long GetLastId()
        {
            return db.NewsLetterSubscribers.Max(x => x.Id);
        }
    }

}
/*











 */
