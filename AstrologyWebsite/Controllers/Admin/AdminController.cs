
﻿using AstrologyWebsite.Migrations;
using System.Text.RegularExpressions;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.CodeAnalysis.Elfie.Model;
using System.Xml.Linq;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using AstrologyWebsite.ViewModels;

[Authorize]

[Authorize]
public class AdminController : Controller
{
    private readonly AstrologyDatabaseContext _context;

    public AdminController(AstrologyDatabaseContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);

        // Get the role ids
        var tarotReaderRoleId = _context.Roles
            .Where(r => r.Name == "TarotReader")
            .Select(r => r.Id)
            .FirstOrDefault();

        var userRoleId = _context.Roles
            .Where(r => r.Name == "User")
            .Select(r => r.Id)
            .FirstOrDefault();

        // Active Readers (TarotReader role)
        var activeReaders = (from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId
                             where userRole.RoleId == tarotReaderRoleId
                                   && user.Status == AccountStatus.Active
                                   && (user.IsDeleted == null || user.IsDeleted == 0)
                             select user).Count();

        // Total Users (User role only, not deleted)
        var totalUsers = (from user in _context.Users
                          join userRole in _context.UserRoles on user.Id equals userRole.UserId
                          where userRole.RoleId == userRoleId
                                && (user.IsDeleted == null || user.IsDeleted == 0)
                          select user).Count();

        // New Users this month (User role only, not deleted)
        var newUsers = (from user in _context.Users
                        join userRole in _context.UserRoles on user.Id equals userRole.UserId
                        where userRole.RoleId == userRoleId
                              && user.CreatedDate >= startOfMonth
                              && (user.IsDeleted == null || user.IsDeleted == 0)
                        select user).Count();

        var model = new AdminDashboardViewModel
        {
            TotalBookings = _context.Bookings.Count(),
            ActiveReaders = activeReaders,
            TotalUsers = totalUsers,
            NewUsers = newUsers,
            BookingStatus = _context.Bookings
                .GroupBy(b => b.Status)
                .Select(g => new BookingStatusCount
                {
                    Status = g.Key.HasValue ? g.Key.Value.ToString() : "Unknown",
                    Count = g.Count()
                })
                .ToList(),
            UpcomingBookings = _context.Bookings
                .Where(b => b.ScheduleAt != null && b.ScheduleAt >= now)
                .OrderBy(b => b.ScheduleAt)
                .Take(10)
                .Select(b => new UpcomingBooking
                {
                    Id = b.Id.ToString(),
                    User = b.Customer != null ? b.Customer.FullName : "Unknown",
                    Reader = b.Tarot != null ? b.Tarot.FullName : "Unknown",
                    Service = b.Service != null ? b.Service.ServiceName : "Unknown",
                    DateTime = b.ScheduleAt.HasValue ? b.ScheduleAt.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    Status = b.Status.HasValue ? b.Status.Value.ToString() : "Unknown"
                })
                .ToList(),
            PopularServices = _context.Bookings
                .Where(b => b.Service != null)
                .GroupBy(b => b.Service.ServiceName)
                .Select(g => new PopularService
                {
                    Name = g.Key,
                    Bookings = g.Count()
                })
                .OrderByDescending(s => s.Bookings)
                .Take(5)
                .ToList(),
            BookingTrends = Enumerable.Range(1, 12)
                .Select(m => _context.Bookings.Count(b =>
                    b.ScheduleAt.HasValue &&
                    b.ScheduleAt.Value.Month == m &&
                    b.ScheduleAt.Value.Year == now.Year))
                .ToList()
        };

        return View(model);
    }

}


