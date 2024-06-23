using Class_system_Backstage_pj.Areas.ordering_system.Data;
using Class_system_Backstage_pj.Data;
using Class_system_Backstage_pj.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<studentContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("studentContext")));
//builder.Services.AddDbContext<studentContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("studentDB")));
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IEmailSender, Emailsender>();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "verification"; // 設定 Session Cookie 的名稱
    options.IdleTimeout = TimeSpan.FromMinutes(10); // 設定 Session 的逾時時間為 10 分鐘
    options.Cookie.HttpOnly = true; // 防止客戶端腳本訪問 Session Cookie
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 要求 Session Cookie 僅透過 HTTPS 傳輸
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
