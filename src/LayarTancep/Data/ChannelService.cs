using Microsoft.EntityFrameworkCore;
using LayarTancep.Data;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class ChannelService : ICrud<Channel>
    {
        LayarTancepDB db;

        public ChannelService()
        {
            if (db == null) db = new LayarTancepDB();

        }

        public bool UnsetNotification(long userid, long channelid)
        {
            try
            {
                var removeItem = db.ChannelNotifications.Where(x => x.UserId == userid && x.ChannelId == channelid).FirstOrDefault();
                if (removeItem != null)
                {
                    db.ChannelNotifications.Remove(removeItem);
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


        public bool SetNotification(long userid, string username, long channelid)
        {
            try
            {
                var newNotif = new ChannelNotification() { CreatedDate = DateHelper.GetLocalTimeNow(), UserName = username, UserId = userid, ChannelId = channelid };
                db.ChannelNotifications.Add(newNotif);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public bool UnSubscribe(long userid, long channelid)
        {
            try
            {
                var removeItem = db.Subscribes.Where(x => x.UserId == userid && x.ChannelId == channelid).FirstOrDefault();
                if (removeItem != null)
                {
                    db.Subscribes.Remove(removeItem);
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


        public bool Subscribe(long userid, string username, long channelid)
        {
            try
            {
                var newLike = new Subscribe() { SubscribeDate  = DateHelper.GetLocalTimeNow(), UserName = username, UserId = userid, ChannelId = channelid };
                db.Subscribes.Add(newLike);
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
            var selData = (db.Channels.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Channels.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<Channel> FindByKeyword(string Keyword)
        {
            var data = from x in db.Channels
                       where x.Name.Contains(Keyword)
                       select x;
            return data.ToList();
        }
        public List<ChannelCategoryCls> GetChannelCategory()
        {
            var datas =  db.Channels.ToList();
            var results = from p in datas
                          group p by p.Category into g
                          select new  ChannelCategoryCls{ PicUrl= g?.First().PicUrl, Category = g.Key, Channels = g.ToList() };
            return results.ToList();

        }
        public List<Channel> GetAllData()
        {
            return db.Channels.OrderBy(x => x.Id).ToList();
        }
        public List<Channel> GetPopular(int Limit = 12)
        {
          
                return db.Channels.Include(c => c.Subscribers).OrderByDescending(x => x.Subscribers.Count).ThenByDescending(x => x.ChannelViews.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            

        }
        public List<Channel> GetByCategory(string Category, string Filter, int Limit = 100)
        {
            if (Filter == FilterChannels.TopViewed)
            {
                return db.Channels.Include(c => c.ChannelViews).Include(c => c.Subscribers).Where(c=>c.Category == Category).OrderByDescending(x => x.ChannelViews.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
            else
            {
                return db.Channels.Include(c => c.Subscribers).Where(c => c.Category == Category).OrderByDescending(x => x.Subscribers.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }

        }
        public List<Channel> GetLatest(string Filter, int Limit=100)
        {
            if(Filter == FilterChannels.TopViewed)
            {
                return db.Channels.Include(c=>c.ChannelViews).Include(c => c.Subscribers).OrderByDescending(x=>x.ChannelViews.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
            else
            {
                return db.Channels.Include(c => c.Subscribers).OrderByDescending(x => x.Subscribers.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
          
        }
        
        public List<Channel> GetAllData(string Filter,string UserName, int Limit=100)
        {
            if(Filter == FilterChannels.TopViewed)
            {
                return db.Channels.Include(c=>c.ChannelViews).Include(c => c.Subscribers).Where(x=>x.UserName == UserName).OrderByDescending(x=>x.ChannelViews.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
            else
            {
                return db.Channels.Include(c => c.Subscribers).Where(x => x.UserName == UserName).OrderByDescending(x => x.Subscribers.Count).ThenByDescending(x => x.CreatedDate).Take(Limit).ToList();
            }
          
        }

        public bool IsChannelExist(string ChannelName)
        {
            var exist = db.Channels.Any(x => x.Name == ChannelName);
            return exist;
        }
        public string GetAvailableName(string ChannelName)
        {
            var BaseName = ChannelName;
            var count = 0;
            do
            {
                var exist = db.Channels.Any(x => x.Name == ChannelName);
                if (exist)
                {
                    ChannelName = $"{BaseName} #{count++}";
                }
                else
                {
                    break;
                }
                Thread.Sleep(100);
            } while (true);
            return ChannelName;
        }
        public List<Channel> GetByUser(UserProfile user)
        {
            
            var datas =  db.Channels.Where(x=>x.UserName == user.Username).OrderBy(x => x.Name).ToList();
            if (!datas.Any())
            {
                var newChannel = new Channel() { UserId = user.Id, UserName = user.Username, CreatedDate = DateHelper.GetLocalTimeNow(), Desc="My Default Channel", Name = GetAvailableName( $"{user.FullName} Videos"), Category = ChannelCategories.Categories[0]   };
                db.Channels.Add(newChannel);
                db.SaveChanges();
                datas = db.Channels.Where(x => x.UserName == user.Username).OrderBy(x => x.Name).ToList();
            }
            
            return datas;
        }

        public Channel GetDataById(object Id)
        {
            return db.Channels.Include(x=>x.Subscribers).Include(c => c.Posts).ThenInclude(x=>x.PostViews).Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Channel data)
        {
            try
            {
                db.Channels.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Channel data)
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
            return db.Channels.Max(x => x.Id);
        }
    }

}
/*











 */
