using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniToDo.Data;
using MiniToDo.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();
app.UseAuthentication();
app.UseAuthorization();


PrintLocalNetworkAddresses(5000);
app.Use(async (context, next) =>
{   
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});
app.Run();


static void PrintLocalNetworkAddresses(int port)
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Сайт доступен по этим адресам в вашей сети:");
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            Console.WriteLine($"→ http://{ip}:{port}");
        }
    }
    Console.ResetColor();
}
