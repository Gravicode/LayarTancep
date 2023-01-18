using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class CommentUnlikeService : ICrud<CommentUnlike>
    {
        LayarTancepDB db;

        public CommentUnlikeService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.CommentUnlikes.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.CommentUnlikes.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<CommentUnlike> FindByKeyword(string Keyword)
        {
            var data = from x in db.CommentUnlikes
                       where x.UnlikedByUserName.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<CommentUnlike> GetAllData()
        {
            return db.CommentUnlikes.OrderBy(x => x.Id).ToList();
        }

        public CommentUnlike GetDataById(object Id)
        {
            return db.CommentUnlikes.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(CommentUnlike data)
        {
            try
            {
                db.CommentUnlikes.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(CommentUnlike data)
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
            return db.CommentUnlikes.Max(x => x.Id);
        }
    }

}
/*











 */
