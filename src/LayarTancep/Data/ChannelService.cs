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

        public List<Channel> GetAllData()
        {
            return db.Channels.OrderBy(x => x.Id).ToList();
        }
        public List<Channel> GetByUser(UserProfile user)
        {
            
            var datas =  db.Channels.Where(x=>x.UserName == user.Username).OrderBy(x => x.Name).ToList();
            if (!datas.Any())
            {
                var newChannel = new Channel() { UserId = user.Id, UserName = user.Username, CreatedDate = DateHelper.GetLocalTimeNow(), Desc="Default Channel", Name = "My Videos", Category = "General"   };
                db.Channels.Add(newChannel);
                db.SaveChanges();
                datas = db.Channels.Where(x => x.UserName == user.Username).OrderBy(x => x.Name).ToList();
            }
            
            return datas;
        }

        public Channel GetDataById(object Id)
        {
            return db.Channels.Where(x => x.Id == (long)Id).FirstOrDefault();
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
