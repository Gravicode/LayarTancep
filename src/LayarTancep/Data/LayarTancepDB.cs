using Microsoft.EntityFrameworkCore;
using LayarTancep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace LayarTancep.Data
{
    public class LayarTancepDB : DbContext
    {

        public LayarTancepDB()
        {
        }

        public LayarTancepDB(DbContextOptions<LayarTancepDB> options)
            : base(options)
        {
        }

        public DbSet<History> Historys { get; set; }     
        public DbSet<ChannelView> ChannelViews { get; set; }     
        public DbSet<ChannelNotification> ChannelNotifications { get; set; }     
        public DbSet<Channel> Channels { get; set; }     
        public DbSet<Trending> Trendings { get; set; }     
        public DbSet<Subscribe> Subscribes { get; set; }     
        public DbSet<PostLike> PostLikes { get; set; }     
        public DbSet<PostView> PostViews { get; set; }     
        public DbSet<PostComment> PostComments { get; set; }     
        public DbSet<CommentLike> CommentLikes { get; set; }     
        public DbSet<Post> Posts { get; set; }     
        public DbSet<UserProfile> UserProfiles { get; set; }     
       
        public DbSet<Notification> Notifications { get; set; }      
      
        public DbSet<Contact> Contacts { get; set; }      
        public DbSet<Log> Logs { get; set; }      
       
       
       
      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
            builder.Entity<DataEventRecord>().HasKey(m => m.DataEventRecordId);
            builder.Entity<SourceInfo>().HasKey(m => m.SourceInfoId);

            // shadow properties
            builder.Entity<DataEventRecord>().Property<DateTime>("UpdatedTimestamp");
            builder.Entity<SourceInfo>().Property<DateTime>("UpdatedTimestamp");
            */
          
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            /*
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<SourceInfo>();
            updateUpdatedProperty<DataEventRecord>();
            */
            return base.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(AppConstants.SQLConn,ServerVersion.AutoDetect(AppConstants.SQLConn));
            }
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            /*
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
            */
        }

    }
}
