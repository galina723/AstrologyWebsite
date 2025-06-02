using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AstrologyWebsite.Models;

public partial class AstrologyDatabaseContext : IdentityDbContext<AstroUser>
{
    public AstrologyDatabaseContext()
    {
    }

    public AstrologyDatabaseContext(DbContextOptions<AstrologyDatabaseContext> options) : base(options)
    {

    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Planet> Planets { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Zodiac> Zodiacs { get; set; }
    //public virtual DbSet<AstroUser> AstroUsers { get; set; }
    public virtual DbSet<Banner> Banners {  get; set; } 

 }

