using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class CommentLikeService : ICrud<CommentLike>
    {
        LayarTancepDB db;

        public CommentLikeService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.CommentLikes.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.CommentLikes.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<CommentLike> FindByKeyword(string Keyword)
        {
            var data = from x in db.CommentLikes
                       where x.LikedByUserName.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<CommentLike> GetAllData()
        {
            return db.CommentLikes.OrderBy(x => x.Id).ToList();
        }

        public CommentLike GetDataById(object Id)
        {
            return db.CommentLikes.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(CommentLike data)
        {
            try
            {
                db.CommentLikes.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(CommentLike data)
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
            return db.CommentLikes.Max(x => x.Id);
        }
    }

}
/*











 */
