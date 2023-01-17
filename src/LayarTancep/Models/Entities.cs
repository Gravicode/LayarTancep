using GemBox.Document;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Reflection;

namespace LayarTancep.Models
{
    #region helpers model
    public class StorageObject
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public DateTime? LastUpdate { get; set; }
        public DateTime? LastAccess { get; set; }
    }
    public class StorageSetting
    {
        public string EndpointUrl { get; set; } = "https://is3.cloudhost.id";
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; } = "USWest1";
        public string Bucket { get; set; }
        public string BaseUrl { get; set; }
        public bool Ssl { get; set; } = true;
        public StorageSetting()
        {

        }
        public StorageSetting(string Endpoint, string Accesskey, string Secretkey, string Region, string Bucket)
        {
            this.EndpointUrl = Endpoint;
            this.AccessKey = Accesskey;
            this.SecretKey = Secretkey;
            this.Region = Region;
            this.Bucket = Bucket;
            GenerateBaseUrl();
        }
        public void GenerateBaseUrl()
        {
            this.BaseUrl = EndpointUrl + "/{bucket}/{key}";
        }
    }
    public class OutputCls
    {
        public OutputCls()
        {
            this.IsSucceed = false;
            this.Message = "";
        }
        public object Data { get; set; }
        public string Message { get; set; }
        public bool IsSucceed { get; set; }
    }
    #endregion

    [Table("history")]
    public class History
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        //user
        [ForeignKey(nameof(User)), Column(Order = 0)]
        public long UserId { get; set; }
        public UserProfile User { set; get; }
        public string UserName { set; get; }

        [ForeignKey(nameof(Post)), Column(Order = 1)]
        public long PostId { get; set; }
        public Post Post { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime UpdatedDate { set; get; }
        public TimeSpan LastWatch { get; set; }
    }
    
    [Table("channel_view")]
    public class ChannelView
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        //user
        [ForeignKey(nameof(User)), Column(Order = 0)]
        public long UserId { get; set; }
        public UserProfile User { set; get; }
        public string UserName { set; get; }

        [ForeignKey(nameof(Channel)), Column(Order = 1)]
        public long ChannelId { get; set; }
        public Channel Channel { set; get; }
        public DateTime CreatedDate { set; get; }
    }

    [Table("channel_notification")]
    public class ChannelNotification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        //user
        [ForeignKey(nameof(User)), Column(Order = 0)]
        public long UserId { get; set; }
        public UserProfile User { set; get; }
        public string UserName { set; get; }

        [ForeignKey(nameof(Channel)), Column(Order = 1)]
        public long ChannelId { get; set; }
        public Channel Channel { set; get; }
        public DateTime CreatedDate { set; get; }
    }


    [Table("channel")]
    public class Channel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Desc { get; set; }
        public string Category { get; set; }
        public string? PicUrl { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Google { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(User)), Column(Order = 0)]
        public long UserId { get; set; }
        public UserProfile User { set; get; }
        public string UserName { set; get; }

        public ICollection<ChannelView> ChannelViews { get; set; }
        public ICollection<Subscribe> Subscribers { get; set; }
        public ICollection<ChannelNotification> ChannelNotifications { get; set; }
        public ICollection<Post> Posts { get; set; }

    }

    [Table("trending")]
    public class Trending
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public string Hashtag { set; get; }
        public long Count { get; set; }
        public DateTime CreatedDate { set; get; }
        public string? Location { set; get; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }

    [Table("subscribe")]
    public class Subscribe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        //user
        [ForeignKey(nameof(User)), Column(Order = 0)]
        public long UserId { get; set; }
        public UserProfile User { set; get; }
        public string UserName { set; get; }   

        [ForeignKey(nameof(Channel)), Column(Order = 1)]
        public long ChannelId { get; set; }
        public Channel Channel { set; get; }
        public DateTime SubscribeDate { set; get; }
    }


    [Table("postlike")]
    public class PostLike
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public Post Post { set; get; }
        [ForeignKey("Post")]
        public long PostId { set; get; }
        [ForeignKey("UserProfile")]
        public long LikedByUserId { set; get; }
        public string LikedByUserName { set; get; }
        public UserProfile LikedByUser { set; get; }
        public DateTime CreatedDate { set; get; }
    }

    [Table("postview")]
    public class PostView
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public Post Post { set; get; }
        [ForeignKey("Post")]
        public long PostId { set; get; }
        [ForeignKey("UserProfile")]
        public long UserId { set; get; }
        public string UserName { set; get; }
        public UserProfile User { set; get; }
        public DateTime CreatedDate { set; get; }
    }

    [Table("postcomment")]
    public class PostComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public string Comment { set; get; }
        public Post Post { set; get; }
        [ForeignKey("Post")]
        public long PostId { set; get; }
        [ForeignKey("UserProfile")]
        public long UserId { set; get; }
        public string Username { set; get; }
        public DateTime CreatedDate { set; get; }
        public UserProfile User { set; get; }
        public ICollection<CommentLike> CommentLikes { get; set; }

        public string CommentLike { get; set; }
        public string CommentUnlike { get; set; }

    }
    [Table("commentlike")]
    public class CommentLike
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public PostComment Comment { set; get; }
        [ForeignKey("Comment")]
        public long CommentId { set; get; }
        [ForeignKey("UserProfile")]
        public long LikedByUserId { set; get; }
        public string LikedByUserName { set; get; }
        public UserProfile LikedByUser { set; get; }
        public DateTime CreatedDate { set; get; }
    }

    [Table("post")]
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [ForeignKey("UserProfile")]
        public long UserId { set; get; }
        public string UserName { set; get; }
        public DateTime CreatedDate { set; get; }
        public string Title { set; get; }
        public string? About { set; get; }
        public string? Tag { set; get; }
        public string? Category { set; get; }
        public string? Privacy { set; get; }
        public string? License { set; get; }
        public string? Language { set; get; }
        public string? Mentions { set; get; }
        public string? VideoUrls { set; get; }
        public string? ImageUrls { set; get; }
        public string? Hashtags { set; get; }
        public TimeSpan? Duration { set; get; }

        [ForeignKey(nameof(Channel)), Column(Order = 1)]
        public long ChannelId { get; set; }
        public Channel Channel { set; get; }
        public UserProfile User { get; set; }

        public ICollection<PostLike> PostLikes { get; set; }
        public ICollection<PostView> PostViews { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
    }

    [Table("notification")]
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }

        public DateTime CreatedDate { set; get; }
        [ForeignKey(nameof(User)), Column(Order = 0)]
        public long UserId { set; get; }
        public UserProfile User { set; get; }
        public string Action { set; get; }
        public string LinkUrl { set; get; }
        public string LinkDesc { set; get; }
        public string Message { set; get; }
        public bool IsRead { set; get; } = false;
    }
    public enum LogCategory
    {
        Info, Error, Warning
    }
    [Table("logs")]
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LogDate { get; set; }
        public string Message { get; set; }
        public LogCategory Category { get; set; }
    }

    [Table("userprofile")]
    public class UserProfile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Alamat { get; set; }
        public string? KTP { get; set; }
        public string? PicUrl { get; set; }
        public bool Aktif { get; set; } = true;
        public string? Daerah { get; set; }
        public string? Desa { get; set; }
        public string? Kelompok { get; set; }
        public Char Gender { get; set; } = 'N';
        public Roles Role { set; get; } = Roles.User;
        public DateTime CreatedDate { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public string? FirstName { set; get; }
        public string? LastName { set; get; }
      
        public string? AboutMe { set; get; }
       
        public string? FBUrl { set; get; }
        public string? TwitterUrl { set; get; }
        public string? InstagramUrl { set; get; }

        public ICollection<Channel> Channels { get; set; }
        public ICollection<PostLike> PostLikes { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<ChannelNotification> ChannelNotifications { get; set; }
        public ICollection<ChannelView> ChannelViews { get; set; }
        public ICollection<History> MyHistory { get; set; }
    }

    [Table("contact")]
    public class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }

        public string FullName { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public string Message { set; get; }
        public DateTime CreatedDate { set; get; }
        public string ReplyMessage { set; get; }
        public string ReplyBy { set; get; }
        public DateTime ReplyDate { set; get; }
    }
  

    public enum Roles { Admin, User, Pengurus }


}
