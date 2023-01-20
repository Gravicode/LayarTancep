using LayarTancep.Data;
using LayarTancep.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Pdf.Content.Objects;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LayarTancep.Helpers;

namespace LayarTancep.Data
{
    public class PostService : ICrud<Post>
    {
        LayarTancepDB db;

        public PostService()
        {
            if (db == null) db = new LayarTancepDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Posts.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Posts.Remove(selData);
            db.SaveChanges();
            return true;
        }
        public bool UnLikePost(long userid, long postid)
        {
            try
            {
                var removePost = db.PostLikes.Where(x => x.LikedByUserId == userid && x.PostId == postid).FirstOrDefault();
                if (removePost != null)
                {
                    db.PostLikes.Remove(removePost);
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }


        public bool LikePost(long userid, string username, long postid)
        {
            try
            {
                var newLike = new PostLike() { CreatedDate = DateHelper.GetLocalTimeNow(), LikedByUserName = username, LikedByUserId = userid, PostId = postid };
                db.PostLikes.Add(newLike);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }
        public List<Post> FindByKeyword(string Keyword)
        {
            var keywords = Keyword?.Split(' ');
            if (keywords.Length > 0)
            {
                var data = from x in db.Posts.Include(c => c.PostViews).Include(c => c.PostComments).Include(c => c.PostLikes).Include(c => c.User).AsEnumerable()
                           where keywords.Any((keystr) => x.Title.Contains(keystr))
                           //x.Message.Contains(Keyword)
                           select x;
                return data.ToList();
            }
            return default;

        }

        public List<string> GetTrendingTags(int Take = 10)
        {
            var listTrend = db.Trendings.GroupBy(info => info.Hashtag)
                       .Select(group => new
                       {
                           hashtag = group.Key,
                           count = group.Count(),
                       }
           ).AsEnumerable()
                       .OrderByDescending(x => x.count).Take(10).ToList();
            var hashtags = new List<string>();
            listTrend.ForEach(x => hashtags.Add(x.hashtag));
            return hashtags.Take(Take).ToList();
        }

        public List<Post> GetTrendingPosts(int Number = 25)
        {

            var listTrend = db.Trendings.GroupBy(info => info.Hashtag)
                        .Select(group => new
                        {
                            hashtag = group.Key,
                            count = group.Count(),
                        }
            ).AsEnumerable()
                        .OrderByDescending(x => x.count).Take(10).ToList();
            var hashtags = new HashSet<string>();
            listTrend.ForEach(x => hashtags.Add(x.hashtag));
            var data = db.Posts.Include(c => c.PostComments).Include(c => c.PostLikes).Include(c => c.User).AsEnumerable().Where(x => LikeHashtag(x.Hashtags)).ToList();
            return data.Take(Number).ToList();
            bool LikeHashtag(string? posthashtag)
            {
                if (posthashtag == null) return false;
                foreach (var hashtag in posthashtag.Split(';'))
                {
                    if (hashtags.Contains(hashtag)) return true;
                }
                return false;
            }
        }
        public List<Post> GetPostMentions(string Username)
        {
            if (string.IsNullOrEmpty(Username)) return default;

            var data = from x in db.Posts.Include(c => c.PostComments).Include(c => c.PostLikes).Include(c => c.User)
                       where x.Mentions.Contains(Username)
                       orderby x.Id descending
                       select x;
            return data.Take(100).ToList();

        }

        public List<Post> GetLikedPosts(string Username)
        {
            if (string.IsNullOrEmpty(Username)) return default;

            var data = from x in db.PostLikes.Include(c => c.Post).Include(c => c.Post.PostComments).Include(c => c.Post.PostLikes).Include(c => c.Post.User)
                       where x.LikedByUserName == Username
                       orderby x.PostId descending
                       select x.Post;
            return data.Take(100).ToList();

        }
        public List<Post> GetMyTimeline(string Username)
        {
            if (string.IsNullOrEmpty(Username)) return default;

            var data = from x in db.Posts.Include(c => c.PostComments).Include(c => c.PostLikes).Include(c => c.User)
                       where x.UserName == Username
                       orderby x.Id descending
                       select x;
            return data.Take(100).ToList();

        }
        public List<Post> GetByUser(string Username, int Limit = 10)
        {
            if (string.IsNullOrEmpty(Username))
                return default;
            var data = from x in db.Posts.Include(c => c.PostComments).Include(c => c.PostLikes).Include(c => c.User)
                       where x.UserName == Username
                       orderby x.CreatedDate descending
                       select x;
            return data.Take(Limit).ToList();
        }

        public List<Post> GetTimeline(string Username)
        {
            if (string.IsNullOrEmpty(Username))
            {
                var data = from x in db.Posts.Include(c => c.PostComments).Include(c => c.PostLikes).Include(c => c.User)

                           orderby x.Id descending
                           select x;
                return data.Take(100).ToList();
            }
            else
            {
                var data = from x in db.Posts.Include(c => c.PostComments).Include(c => c.PostLikes).Include(c => c.User)
                           where x.UserName == Username
                           orderby x.Id descending
                           select x;
                var followChannel = db.Subscribes.Where(x => x.UserName == Username).Select(x => x.ChannelId).ToList();
                var data2 = from x in db.Posts.Include(c => c.PostComments).Include(c => c.PostLikes).Include(c => c.User)
                            where followChannel.Contains(x.ChannelId)
                            orderby x.Id descending
                            select x;
                var union = data.Union(data2).OrderByDescending(x => x.Id);
                return union.Take(100).ToList();
            }
        }
        public List<Post> GetByChannel(long ChannelId)
        {
            var datas = db.Posts.Include(c => c.PostViews).Include(c => c.PostLikes).Include(c => c.PostComments).Where(x => x.ChannelId == ChannelId).OrderByDescending(x => x.Id).ToList();

            return datas;
        }
        public List<Post> GetSimilar(Post item)
        {
            var datas = db.Posts.Include(c => c.PostViews).Include(c => c.PostLikes).Include(c => c.PostComments).Where(x => x.Category == item.Category).OrderBy(x => x.Id).ToList();
            if (datas.Count > 0 && !string.IsNullOrEmpty(item.Tag))
            {
                var tags = item.Tag.Split(",");
                if (tags.Length > 0)
                {
                    var filtered = datas.Where(x => x.Tag.ContainsAny(tags)).ToList();
                    if (filtered.Count > 0)
                        return filtered;
                }
            }
            return datas;
        }
        public List<Post> GetAllData()
        {
            return db.Posts.OrderBy(x => x.Id).ToList();
        }
        public List<Post> GetByCategory(string Category, string Filter, int Limit = 12)
        {
            if (Filter == FilterChannels.TopViewed)
            {
                return db.Posts.Include(c => c.PostViews).Where(c=>c.Category == Category).OrderByDescending(x => x.PostViews.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
            else
            {
                return db.Posts.Include(c => c.PostLikes).Include(c => c.PostViews).Where(c => c.Category == Category).OrderByDescending(x => x.PostLikes.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
        }
        public List<Post> GetFeatured(string Filter, int Limit = 12)
        {
            if (Filter == FilterChannels.TopViewed)
            {
                return db.Posts.Include(c => c.PostViews).OrderByDescending(x => x.PostViews.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
            else
            {
                return db.Posts.Include(c => c.PostLikes).Include(c => c.PostViews).OrderByDescending(x => x.PostLikes.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
        } 
        
        public List<Post> GetAllData(string Filter, string UserName, int Limit = 1000)
        {
            if (Filter == FilterChannels.TopViewed)
            {
                return db.Posts.Include(c => c.PostViews).Where(x=>x.UserName == UserName).OrderByDescending(x => x.PostViews.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
            else
            {
                return db.Posts.Include(c => c.PostLikes).Include(c => c.PostViews).Where(x => x.UserName == UserName).OrderByDescending(x => x.PostLikes.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
        }

        public Post GetDataById(object Id)
        {
            return db.Posts.Include(c => c.Channel)
                .ThenInclude(c => c.Subscribers)
                .Include(c => c.Channel)
                    .ThenInclude(c => c.ChannelNotifications)
                .Include(c => c.Channel)
                    .ThenInclude(c => c.Posts)
                .Include(c => c.PostLikes)
                .Include(c => c.PostComments)
                    .ThenInclude(c => c.User)
                .Include(c => c.PostComments)
                    .ThenInclude(c => c.CommentLikes)
                .Include(c => c.PostComments)
                    .ThenInclude(c => c.CommentUnlikes)
                .Include(c => c.PostViews).Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Post data)
        {
            try
            {
                db.Posts.Add(data);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;

        }



        public bool UpdateData(Post data)
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
            return db.Posts.Max(x => x.Id);
        }
    }

}
/*











 */
