using Blazored.LocalStorage;
using Blazored.Toast;
using LayarTancep.Tools;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using LayarTancep.Data;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;
using PdfSharp.Charting;
using System.Net;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using LayarTancep.Models;
using LayarTancep.Data;
using Xabe.FFmpeg.Downloader;
using Xabe.FFmpeg;

var builder = WebApplication.CreateBuilder(args);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// ******
// BLAZOR COOKIE Auth Code (begin)
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
// BLAZOR COOKIE Auth Code (end)
// ******
// ******
// BLAZOR COOKIE Auth Code (begin)
// From: https://github.com/aspnet/Blazor/issues/1554
// HttpContextAccessor
//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddTransient<AzureBlobHelper>();
builder.Services.AddTransient<LogService>();
builder.Services.AddTransient<UserProfileService>();
builder.Services.AddTransient<ContactService>();
builder.Services.AddTransient<NotificationService>();
builder.Services.AddTransient<ChannelViewService>();
builder.Services.AddTransient<ChannelNotificationService>();
builder.Services.AddTransient<ChannelService>();
builder.Services.AddTransient<TrendingService>();
builder.Services.AddTransient<SubscribeService>();
builder.Services.AddTransient<PostLikeService>();
builder.Services.AddTransient<PostViewService>();
builder.Services.AddTransient<PostCommentService>();
builder.Services.AddTransient<CommentLikeService>();
builder.Services.AddTransient<PostService>();
builder.Services.AddTransient<HistoryService>();
builder.Services.AddTransient<CommentUnlikeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET, PATCH, DELETE, PUT, POST, OPTIONS"));
});
var configBuilder = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("appsettings.json", optional: false);
IConfiguration Configuration = configBuilder.Build();

AppConstants.ProxyIP = Configuration["ProxyIP"];

var proxies = AppConstants.ProxyIP.Split(';');
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        foreach (var proxy in proxies)
        {
            options.KnownProxies.Add(IPAddress.Parse(proxy));
        }
    });



AppConstants.UploadUrlPrefix = Configuration["UploadUrlPrefix"];
AppConstants.SQLConn = Configuration["ConnectionStrings:SqlConn"];
AppConstants.RedisCon = Configuration["RedisCon"];
AppConstants.BlobConn = Configuration["ConnectionStrings:BlobConn"];
AppConstants.GMapApiKey = Configuration["GmapKey"];
AppConstants.FFMpegFolder = Configuration["FFMpegFolder"];
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();

MailService.MailUser = Configuration["MailSettings:MailUser"];
MailService.MailPassword = Configuration["MailSettings:MailPassword"];
MailService.MailServer = Configuration["MailSettings:MailServer"];
MailService.MailPort = int.Parse(Configuration["MailSettings:MailPort"]);
MailService.SetTemplate(Configuration["MailSettings:TemplatePath"]);
MailService.SendGridKey = Configuration["MailSettings:SendGridKey"];
MailService.UseSendGrid = true;


SmsService.UserKey = Configuration["SmsSettings:ZenzivaUserKey"];
SmsService.PassKey = Configuration["SmsSettings:ZenzivaPassKey"];
SmsService.TokenKey = Configuration["SmsSettings:TokenKey"];

AppConstants.StorageEndpoint = Configuration["Storage:Endpoint"];
AppConstants.StorageAccess = Configuration["Storage:Access"];
AppConstants.StorageSecret = Configuration["Storage:Secret"];
AppConstants.StorageBucket = Configuration["Storage:Bucket"];
var setting = new StorageSetting() { };
setting.Bucket = AppConstants.StorageBucket;
setting.SecretKey = AppConstants.StorageSecret;
setting.AccessKey = AppConstants.StorageAccess;

builder.Services.AddSingleton(setting);
builder.Services.AddTransient<StorageObjectService>();

AppConstants.DefaultPass = Configuration["App:DefaultPass"];

await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official,AppConstants.FFMpegFolder);
FFmpeg.SetExecutablesPath(AppConstants.FFMpegFolder);

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 128 * 1024; // 1MB
});


var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    //app.UseHttpsRedirection();
    StaticWebAssetsLoader.UseStaticWebAssets(
              app.Environment,
              app.Configuration);

}


app.UseStaticFiles();

app.UseRouting();

// ******
// BLAZOR COOKIE Auth Code (begin)
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
// BLAZOR COOKIE Auth Code (end)
// ******

app.UseCors(x => x
.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true) // allow any origin  
.AllowCredentials());               // allow credentials 

// BLAZOR COOKIE Auth Code (begin)
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
// BLAZOR COOKIE Auth Code (end)

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var db = new LayarTancepDB();
db.Database.EnsureCreated();


app.Run();
