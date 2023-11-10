using Microsoft.EntityFrameworkCore;
using Project.Models;
var builder = WebApplication.CreateBuilder(args);

string conStr = builder.Configuration.GetConnectionString("MyConStr");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NorthWindContext>(options => options.UseSqlServer(conStr));
var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Product}/{action=Index}");
app.Run();
