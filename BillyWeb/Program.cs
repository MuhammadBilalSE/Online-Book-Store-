using Billy.DataAccess.Data;
using Billy.DataAccess.Repository;
using Billy.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Billy.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BulkyDB>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("dbconnection")));

builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<BulkyDB>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(
    option =>
    {
        option.LoginPath = $"/Identity/Account/Login";
        option.LoginPath = $"/Identity/Account/Logout";
        option.LoginPath = $"/Identity/Account/AccessDenied";
    });

builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitofWork, UnitofWork>() ;
builder.Services.AddScoped<IEmailSender, SendEmail>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
