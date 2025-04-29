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
    options.Password.RequireDigit = false; // Kh�ng b?t ph?i c� s?
    options.Password.RequireLowercase = false; // Kh�ng b?t ph?i c� ch? th??ng
    options.Password.RequireNonAlphanumeric = false; // Kh�ng b?t k� t? ??c bi?t
    options.Password.RequireUppercase = false; // Kh�ng b?t bu?c ch? in
    options.Password.RequiredLength = 3; // S? k� t? t?i thi?u c?a password
    options.Password.RequiredUniqueChars = 1; // S? k� t? ri�ng bi?t

    // C?u h�nh Lockout - kh�a user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Kh�a 5 ph�t
    options.Lockout.MaxFailedAccessAttempts = 3; // Th?t b?i 3 l? th� kh�a
    options.Lockout.AllowedForNewUsers = true;

    // C?u h�nh v? User.
    options.User.AllowedUserNameCharacters = // c�c k� t? ??t t�n user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email l� duy nh?t


    // C?u h�nh ??ng nh?p.
    options.SignIn.RequireConfirmedEmail = true;            // C?u h�nh x�c th?c ??a ch? email (email ph?i t?n t?i)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // X�c th?c s? ?i?n tho?i
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
