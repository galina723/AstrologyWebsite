using System;
using System.Configuration;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AstrologyDatabaseContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("AstrologyDatabase"));
});

builder.Services.AddIdentity<AstroUser, IdentityRole>()
              .AddEntityFrameworkStores<AstrologyDatabaseContext>()
              .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options => {
    // Thi?t l?p v? Password
    options.Password.RequireDigit = false; // Không b?t ph?i có s?
    options.Password.RequireLowercase = false; // Không b?t ph?i có ch? th??ng
    options.Password.RequireNonAlphanumeric = false; // Không b?t ký t? ??c bi?t
    options.Password.RequireUppercase = false; // Không b?t bu?c ch? in
    options.Password.RequiredLength = 3; // S? ký t? t?i thi?u c?a password
    options.Password.RequiredUniqueChars = 1; // S? ký t? riêng bi?t

    // C?u hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Th?t b?i 3 l? thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // C?u hình v? User.
    options.User.AllowedUserNameCharacters = // các ký t? ??t tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nh?t


    // C?u hình ??ng nh?p.
    options.SignIn.RequireConfirmedEmail = true;            // C?u hình xác th?c ??a ch? email (email ph?i t?n t?i)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác th?c s? ?i?n tho?i
    options.SignIn.RequireConfirmedAccount = true;

    });

    builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/khongduoctruycap.html";
    });

builder.Services.AddAuthentication();
var configuration = builder.Configuration;

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var gconfig = configuration.GetSection("Authentication:Google");
        options.ClientId = gconfig["ClientId"];
        options.ClientSecret = gconfig["ClientSecret"];
        options.CallbackPath = "/dang-nhap-tu-google";
    })
    .AddFacebook(options =>
    {
        var fconfig = configuration.GetSection("Authentication:Facebook");
        options.AppId = fconfig["AppId"];
        options.AppSecret = fconfig["AppSecret"];
        options.CallbackPath = "/dang-nhap-tu-facebook";
    });

builder.Services.AddControllersWithViews();

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
