using Comercio.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Database:Connection"]);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(50);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddControllersWithViews();

var app = builder.Build();

//Middleware ->
app.MapControllerRoute("default", "{controller=Home}/{action=Index}");

app.UseSession();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
