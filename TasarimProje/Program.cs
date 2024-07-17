using DAL.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using TasarimProje.Controllers;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<VtContext>(_ => _.UseSqlServer("Server=localhost;Database=vt;Trusted_Connection=True; TrustServerCertificate=True")); //            => optionsBuilder.UseSqlServer("Server=localhost;Database=vt;Trusted_Connection=True; TrustServerCertificate=True");
//diðer contoller a girevilmesi için
builder.Services.AddControllersWithViews(); 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    //options.Cookie.Name = "";
    options.LoginPath = "/home/index";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);//60dakika sonra cookie iþlemi btsin


});

builder.Services.AddHttpContextAccessor();
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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index2}/{id?}");

app.Run();
