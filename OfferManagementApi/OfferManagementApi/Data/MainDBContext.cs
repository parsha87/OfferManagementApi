using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OfferManagementApi.Data.Entities;

namespace OfferManagementApi.Data;

public partial class MainDBContext : DbContext
{
    public MainDBContext()
    {
    }

    public MainDBContext(DbContextOptions<MainDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Frequency> Frequencies { get; set; }

    public virtual DbSet<Inquiry> Inquiries { get; set; }

    public virtual DbSet<InquiryAttachmentsRecord> InquiryAttachmentsRecords { get; set; }

    public virtual DbSet<ListOfValue> ListOfValues { get; set; }

    public virtual DbSet<TechnicalDetailsMapping> TechnicalDetailsMappings { get; set; }

    public virtual DbSet<VisitSection> VisitSections { get; set; }

    public virtual DbSet<Voltage> Voltages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AI");

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AspNetRo__3214EC07B18218C7");
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AspNetRo__3214EC07C5D7733F");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasConstraintName("FK__AspNetRol__RoleI__47DBAE45");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AspNetUs__3214EC074134F47E");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__AspNetUse__RoleI__3C69FB99"),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__AspNetUse__UserI__3B75D760"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__AspNetUs__AF2760ADC8AF24D9");
                        j.ToTable("AspNetUserRoles");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AspNetUs__3214EC0755448237");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasConstraintName("FK__AspNetUse__UserI__3F466844");
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey }).HasName("PK__AspNetUs__2B2C5B526CCDF56F");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__AspNetUse__UserI__4222D4EF");
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name }).HasName("PK__AspNetUs__8CC498411FE3E78F");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasConstraintName("FK__AspNetUse__UserI__44FF419A");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brands__3214EC071B5797D1");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07328B3B65");
        });

        modelBuilder.Entity<Inquiry>(entity =>
        {
            entity.HasKey(e => e.InquiryId).HasName("PK__Inquirie__05E6E7CF6E362673");
        });

        modelBuilder.Entity<InquiryAttachmentsRecord>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("PK__InquiryA__442C64BE041EB285");

            entity.Property(e => e.UploadedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Inquiry).WithMany(p => p.InquiryAttachmentsRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inquiry_Attachments");
        });

        modelBuilder.Entity<ListOfValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ListOfVa__3214EC073BBDB695");
        });

        modelBuilder.Entity<TechnicalDetailsMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Technica__3214EC07D44993E5");

            entity.HasOne(d => d.Inquiry).WithMany(p => p.TechnicalDetailsMappings).HasConstraintName("FK__Technical__Inqui__0A9D95DB");
        });

        modelBuilder.Entity<VisitSection>(entity =>
        {
            entity.HasKey(e => e.VisitSectionId).HasName("PK__VisitSec__EA5EEBBBB4A6EB5A");

            entity.HasOne(d => d.Inquiry).WithMany(p => p.VisitSections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VisitSection_Inquiry");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
