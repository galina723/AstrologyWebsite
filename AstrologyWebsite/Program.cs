﻿using System;
using System.Configuration;
using AstrologyWebsite.HoroscropServices;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AstrologyDatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AstrologyDatabase"));
});
builder.Services.AddSingleton<ZodiacCompatibilityService>();

builder.Services.AddIdentity<AstroUser, IdentityRole>()
              .AddEntityFrameworkStores<AstrologyDatabaseContext>()
              .AddDefaultTokenProviders();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 5; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất


    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;

});

builder.Services.ConfigureApplicationCookie(options =>
{
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<AstroUser>>();

    string[] roles = { "Admin", "User", "TarotReader" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // default admin user
    var adminEmail = "admin@gmail.com";
    var adminPassword = "admin123@";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var user = new AstroUser
        {
            FullName = "Amelia",
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            IsDeleted = 0,
            Status = AccountStatus.Active
        };

        var result = await userManager.CreateAsync(user, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }

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
        name: "admin_default",
        pattern: "{controller=Admin}/{action}/{id?}"
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
}
