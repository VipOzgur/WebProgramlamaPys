using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebFinalPys.Models;

public partial class PysDbContext : DbContext
{
    public PysDbContext()
    {
    }

    public PysDbContext(DbContextOptions<PysDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Izinler> Izinlers { get; set; }

    public virtual DbSet<MaasPrim> MaasPrims { get; set; }

    public virtual DbSet<MaasZamlari> MaasZamlaris { get; set; }

    public virtual DbSet<Mesailer> Mesailers { get; set; }

    public virtual DbSet<Personel> Personels { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.UseSqlite("Data Source=.//Data//PysDb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_Departments_Id").IsUnique();
        });

        modelBuilder.Entity<Izinler>(entity =>
        {
            entity.ToTable("Izinler");

            entity.HasIndex(e => e.Id, "IX_Izinler_Id").IsUnique();

            entity.HasOne(d => d.Admin).WithMany(p => p.IzinlerAdmins)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Personel).WithMany(p => p.IzinlerPersonels)
                .HasForeignKey(d => d.PersonelId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MaasPrim>(entity =>
        {
            entity.ToTable("MaasPrim");

            entity.HasIndex(e => e.Id, "IX_MaasPrim_Id").IsUnique();

            entity.HasOne(d => d.Admin).WithMany(p => p.MaasPrimAdmins)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Personel).WithMany(p => p.MaasPrimPersonels)
                .HasForeignKey(d => d.PersonelId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MaasZamlari>(entity =>
        {
            entity.ToTable("MaasZamlari");

            entity.HasIndex(e => e.Id, "IX_MaasZamlari_Id").IsUnique();

            entity.HasOne(d => d.Admin).WithMany(p => p.MaasZamlariAdmins)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Personel).WithMany(p => p.MaasZamlariPersonels)
                .HasForeignKey(d => d.PersonelId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Mesailer>(entity =>
        {
            entity.ToTable("Mesailer");

            entity.HasIndex(e => e.Id, "IX_Mesailer_Id").IsUnique();

            entity.HasOne(d => d.Admin).WithMany(p => p.MesailerAdmins)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PersonelNavigation).WithMany(p => p.MesailerPersonelNavigations)
                .HasForeignKey(d => d.Personel)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Personel>(entity =>
        {
            entity.ToTable("Personel");

            entity.HasIndex(e => e.Id, "IX_Personel_Id").IsUnique();

            entity.HasOne(d => d.Dep).WithMany(p => p.Personels).HasForeignKey(d => d.DepId);

            entity.HasOne(d => d.Role).WithMany(p => p.Personels)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_Roles_Id").IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
