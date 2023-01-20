using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LayarTancep.Helpers;

namespace LayarTancep.Data
{
    public class BlogService : ICrud<Blog>
    {
        LayarTancepDB db;

        public BlogService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Blogs.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Blogs.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<Blog> FindByKeyword(string Keyword)
        {
            var data = from x in db.Blogs
                       where x.Title.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<Blog> GetAllData()
        {
            return db.Blogs.OrderBy(x => x.Id).ToList();
        }  
        
        public List<Blog> GetRelated(Blog item,int Limit=2)
        {
            return db.Blogs.ToList().Where(x => x.Tags.ContainsAny(item.Tags.Split(';'))).OrderByDescending(x => x.CreatedDate).Take(Limit).OrderBy(x => x.Id).ToList();
        } 
        
        public List<string> GetTags()
        {
            var taglist =  db.Blogs.Select(x=>x.Tags).ToList();
            HashSet<string> Tags = new();
            foreach(var tag in taglist)
            {
                foreach(var item in tag.Split(';'))
                {
                    if (!Tags.Contains(item.ToLower()))
                    {
                        Tags.Add(item.ToLower());
                    }
                }
                
            }
            return Tags.ToList();
        }
        
        public List<Blog> GetTrending(int Limit=5)
        {
			return db.Blogs.Include(c=>c.BlogComments).OrderByDescending(x=>x.BlogComments.Count()).OrderByDescending(x => x.CreatedDate).Take(Limit).OrderBy(x => x.Id).ToList();
		}
        public List<Blog> GetFeatured(int Limit=10)
        {
            return db.Blogs.Where(x=>x.Featured).OrderByDescending(x => x.CreatedDate).Take(Limit).OrderBy(x => x.Id).ToList();
        }
        public List<Blog> GetLatest(string Keyword, int Limit = 30)
        {
            return string.IsNullOrEmpty(Keyword)? db.Blogs.OrderByDescending(x => x.CreatedDate).Take(Limit).ToList() : db.Blogs.Where(x=>x.Title.Contains(Keyword)).OrderByDescending(x => x.CreatedDate).Take(Limit).ToList();
        }
        public List<Blog> GetByTag(string Tag, int Limit = 30)
        {
            return string.IsNullOrEmpty(Tag)? db.Blogs.OrderByDescending(x => x.CreatedDate).Take(Limit).ToList() : db.Blogs.Where(x=>x.Tags.Contains(Tag)).OrderByDescending(x => x.CreatedDate).Take(Limit).ToList();
        }
        public List<Blog> GetByCategory(string Category, int Limit = 30)
        {
            return string.IsNullOrEmpty(Category)? db.Blogs.OrderByDescending(x => x.CreatedDate).Take(Limit).ToList() : db.Blogs.Where(x=>x.Category.Contains(Category)).OrderByDescending(x => x.CreatedDate).Take(Limit).ToList();
        }
        public List<Blog> GetByMonthYear(int Month, int Year, int Limit = 30)
        {
            return db.Blogs.Where(x=>x.CreatedDate.Month == Month && x.CreatedDate.Year == Year).OrderByDescending(x => x.CreatedDate).Take(Limit).ToList();
        }

        public Blog GetDataById(object Id)
        {
            return db.Blogs.Include(c=>c.BlogComments).Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Blog data)
        {
            try
            {
                db.Blogs.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Blog data)
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
            return db.Blogs.Max(x => x.Id);
        }
    }

}
/*











 */
