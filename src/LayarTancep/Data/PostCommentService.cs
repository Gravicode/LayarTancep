using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class PostCommentService : ICrud<PostComment>
    {
        LayarTancepDB db;

        public PostCommentService()
        {
            if (db == null) db = new LayarTancepDB();

        }

        public bool UnLikeComment(long userid, string username, long commentid)
        {
            try
            {
                var newUnLike = new CommentUnlike() { CreatedDate = DateHelper.GetLocalTimeNow(), UnlikedByUserName = username, UnlikedByUserId = userid, CommentId = commentid };
                db.CommentUnlikes.Add(newUnLike);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }


        public bool LikeComment(long userid, string username, long commentid)
        {
            try
            {
                var newLike = new CommentLike() { CreatedDate = DateHelper.GetLocalTimeNow(), LikedByUserName = username, LikedByUserId = userid, CommentId = commentid };
                db.CommentLikes.Add(newLike);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }
        public bool DeleteData(object Id)
        {
            var selData = (db.PostComments.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.PostComments.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<PostComment> FindByKeyword(string Keyword)
        {
            var data = from x in db.PostComments.Include(c=>c.Post)
                       where x.Post.Title.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<PostComment> GetAllData()
        {
            return db.PostComments.OrderBy(x => x.Id).ToList();
        }

        public PostComment GetDataById(object Id)
        {
            return db.PostComments.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(PostComment data)
        {
            try
            {
                db.PostComments.Add(data);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;

        }



        public bool UpdateData(PostComment data)
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
            return db.PostComments.Max(x => x.Id);
        }
    }

}
/*











 */
