﻿using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class PostLikeService : ICrud<PostLike>
    {
        LayarTancepDB db;

        public PostLikeService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.PostLikes.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.PostLikes.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<PostLike> FindByKeyword(string Keyword)
        {
            var data = from x in db.PostLikes.Include(c=>c.LikedByUser)
                       where x.Post.User.Username.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<PostLike> GetAllData()
        {
            return db.PostLikes.OrderBy(x => x.Id).ToList();
        }

        public PostLike GetDataById(object Id)
        {
            return db.PostLikes.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(PostLike data)
        {
            try
            {
                db.PostLikes.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(PostLike data)
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
            return db.PostLikes.Max(x => x.Id);
        }
    }

}
/*











 */
