using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AstrologyWebsite.Data;

public partial class AstrologyDatabaseContext : DbContext
{
    public AstrologyDatabaseContext()
    {
    }

    public AstrologyDatabaseContext(DbContextOptions<AstrologyDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogDetail> BlogDetails { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Planet> Planets { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zodiac> Zodiacs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-LS2KQER;Initial Catalog=AstrologyDatabase;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blogs__3214EC27E9FD9A80");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__Blogs__AuthorId__5441852A");
        });

        modelBuilder.Entity<BlogDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blog_det__3214EC2700F04EE8");

            entity.ToTable("Blog_detail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogDetails)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__Blog_deta__blog___571DF1D5");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC271F53FE9C");

            entity.ToTable("Booking");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("note");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ScheduleAt)
                .HasColumnType("datetime")
                .HasColumnName("schedule_at");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TarotId).HasColumnName("tarot_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.BookingCustomers)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Booking__custome__5DCAEF64");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Booking__service__5FB337D6");

            entity.HasOne(d => d.Tarot).WithMany(p => p.BookingTarots)
                .HasForeignKey(d => d.TarotId)
                .HasConstraintName("FK__Booking__tarot_i__5EBF139D");
        });

        modelBuilder.Entity<Planet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Planets__3214EC2727FA5A5C");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Symbol)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("symbol");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC27D20A9702");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3214EC277C2ADE27");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("service_name");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27307110D4");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__role_id__4AB81AF0");
        });

        modelBuilder.Entity<Zodiac>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Zodiacs__3214EC276BECA18B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Element)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("element");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.Modality)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modality");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.Symbol)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("symbol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
