using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class PostViewService : ICrud<PostView>
    {
        LayarTancepDB db;

        public PostViewService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.PostViews.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.PostViews.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<PostView> FindByKeyword(string Keyword)
        {
            var data = from x in db.PostViews.Include(c=>c.Post)
                       where x.Post.Title.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<PostView> GetAllData()
        {
            return db.PostViews.OrderBy(x => x.Id).ToList();
        }

        public PostView GetDataById(object Id)
        {
            return db.PostViews.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(PostView data)
        {
            try
            {
                db.PostViews.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(PostView data)
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
            return db.PostViews.Max(x => x.Id);
        }
    }

}
/*











 */
