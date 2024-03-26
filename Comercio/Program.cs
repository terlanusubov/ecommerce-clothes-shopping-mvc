using Comercio.Data;
using Comercio.Notifications.Slack;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<SlackService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Database:Connection"]);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(50);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,config =>
    {
        config.LoginPath = $"/Account/Login";

        config.ExpireTimeSpan = TimeSpan.FromDays(1);

        config.SlidingExpiration = false;

        config.Cookie.IsEssential = true;
    })
    .AddCookie("AdminCookies",config =>
    {
        config.LoginPath = $"/Admin/Account/Login";

        config.ExpireTimeSpan = TimeSpan.FromDays(1);

        config.SlidingExpiration = false;

        config.Cookie.IsEssential = true;
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

//Middleware ->


//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Proqrama request gelir!");
//    await next();
//    Console.WriteLine("Proqrama response gedir!");

//});

app.UseSession();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}");

app.MapControllerRoute("default", "{controller=Home}/{action=Index}");

app.Run();
