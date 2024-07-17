using System;
using System.Collections.Generic;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Contexts
{
    public partial class VtContext : DbContext
    {
        public VtContext()
        {
        }

        public VtContext(DbContextOptions<VtContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Favori> Favori { get; set; }

        public virtual DbSet<Kategori> Kategori { get; set; }

        public virtual DbSet<Material> Material { get; set; }

        public virtual DbSet<Pm> Pm { get; set; }

        public virtual DbSet<Project> Project { get; set; }

        public virtual DbSet<ProjectYorum> ProjectYorum { get; set; }

        public virtual DbSet<Rol> Rol { get; set; }

        public virtual DbSet<Tur> Tur { get; set; }

        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=localhost;Database=vt;Trusted_Connection=True; TrustServerCertificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favori>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__Favori__3214D4A8F7CB3F29");

                entity.Property(e => e.EklemeTarih).HasColumnType("datetime");

                entity.HasOne(d => d.ProjectNoNavigation).WithMany(p => p.Favori)
                    .HasForeignKey(d => d.ProjectNo)
                    .HasConstraintName("FK__Favori__ProjectN__4AB81AF0");

                entity.HasOne(d => d.UsersNoNavigation).WithMany(p => p.Favori)
                    .HasForeignKey(d => d.UsersNo)
                    .HasConstraintName("FK__Favori__UsersNo__4BAC3F29");
            });

            modelBuilder.Entity<Kategori>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__Kategori__3214D4A8C9CFCBF2");

                entity.Property(e => e.Ad)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__Material__3214D4A8DA9240E2");

                entity.Property(e => e.Baslik)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.KategoriNoNavigation).WithMany(p => p.Material)
                    .HasForeignKey(d => d.KategoriNo)
                    .HasConstraintName("FK__Material__Katego__403A8C7D");
            });

            modelBuilder.Entity<Pm>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__PM__3214D4A8884F0F7B");

                entity.ToTable("PM");

                entity.HasOne(d => d.MaterialNoNavigation).WithMany(p => p.Pm)
                    .HasForeignKey(d => d.MaterialNo)
                    .HasConstraintName("FK__PM__MaterialNo__59FA5E80");

                entity.HasOne(d => d.ProjectNoNavigation).WithMany(p => p.Pm)
                    .HasForeignKey(d => d.ProjectNo)
                    .HasConstraintName("FK__PM__ProjectNo__59063A47");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__Project__3214D4A8353E9F9B");

                entity.Property(e => e.Ad)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Explain)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.MaterialNoNavigation).WithMany(p => p.Project)
                    .HasForeignKey(d => d.MaterialNo)
                    .HasConstraintName("FK__Project__Materia__4316F928");

                entity.HasOne(d => d.TurNoNavigation).WithMany(p => p.Project)
                    .HasForeignKey(d => d.TurNo)
                    .HasConstraintName("FK__Project__TurNo__440B1D61");
            });

            modelBuilder.Entity<ProjectYorum>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__ProjectY__3214D4A86E14D323");

                entity.Property(e => e.Aciklama)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Konu)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProjectNoNavigation).WithMany(p => p.ProjectYorum)
                    .HasForeignKey(d => d.ProjectNo)
                    .HasConstraintName("FK__ProjectYo__Proje__46E78A0C");

                entity.HasOne(d => d.UsersNoNavigation).WithMany(p => p.ProjectYorum)
                    .HasForeignKey(d => d.UsersNo)
                    .HasConstraintName("FK__ProjectYo__Users__47DBAE45");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__Rol__3214D4A8535DBB66");

                entity.Property(e => e.Ad)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tur>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__Tur__3214D4A8B5CF61BA");

                entity.Property(e => e.Ad)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.No).HasName("PK__Users__3214D4A81CCC8414");

                entity.Property(e => e.AdSoyad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Ad_Soyad");
                entity.Property(e => e.Eposta)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.KullaniciAd)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Sifre)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.RolNoNavigation).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RolNo)
                    .HasConstraintName("FK__Users__RolNo__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
