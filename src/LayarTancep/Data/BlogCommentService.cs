using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class BlogCommentService : ICrud<BlogComment>
    {
        LayarTancepDB db;

        public BlogCommentService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.BlogComments.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.BlogComments.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<BlogComment> FindByKeyword(string Keyword)
        {
            var data = from x in db.BlogComments
                       where x.Comment.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<BlogComment> GetAllData()
        {
            return db.BlogComments.OrderBy(x => x.Id).ToList();
        }

        public BlogComment GetDataById(object Id)
        {
            return db.BlogComments.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(BlogComment data)
        {
            try
            {
                db.BlogComments.Add(data);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;

        }



        public bool UpdateData(BlogComment data)
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
            return db.BlogComments.Max(x => x.Id);
        }
    }

}
/*











 */
