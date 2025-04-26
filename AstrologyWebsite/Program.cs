using AstrologyWebsite.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AstrologyDatabaseContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("AstrologyDatabase"));
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "details",
    pattern: "{controller=Details}/{action}/{id?}"
);

app.MapControllerRoute(
    name: "admin_default",
    pattern: "{controller=Admin}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "admin_dynamic",
    pattern: "Admin/{controller=Details}/{action}/{id?}"
);

app.Run();
