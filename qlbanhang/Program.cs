using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using qlbanhang.Model1s;
using qlbanhang.Models;

var builder = WebApplication.CreateBuilder(args);
//https://github.com/aspnetcorehero/ToastNotification
// Add services to the container.
builder.Services.AddControllersWithViews();

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
//builder.Services.AddDbContext<DEmployeeFdbContext>(options => options.UseFirebird(configuration.GetConnectionString("TestNorthWind")));
builder.Services.AddDbContext<DNorthwindFdbContext>(options => options.UseFirebird(configuration.GetConnectionString("TestNorthWind2")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddNotyf(options =>
{
    options.Position = NotyfPosition.TopRight;
    options.DurationInSeconds = 10;
    options.IsDismissable = true;   
});
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
app.UseNotyf();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=listProduct}/{id?}");

app.Run();
