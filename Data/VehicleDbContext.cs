using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using VehicleController.Models;

namespace VehicleController.Data
{
    public partial class VehicleDbContext : DbContext
    {
        public VehicleDbContext()
        {
        }

        public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<Make> Makes { get; set; } = null!;
        public virtual DbSet<Model> Models { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Trip> Trips { get; set; } = null!;
        public virtual DbSet<Truck> Trucks { get; set; } = null!;
        public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name = DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Make)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.MakeId)
                    .HasConstraintName("FK__Car__MakeId__32E0915F");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ModelId)
                    .HasConstraintName("FK__Car__ModelId__33D4B598");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK__Car__VehicleId__31EC6D26");
            });

            modelBuilder.Entity<Make>(entity =>
            {
                entity.ToTable("Make");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("Model");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Make)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.MakeId)
                    .HasConstraintName("FK__Model__MakeId__267ABA7A");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.HasIndex(e => e.Name, "UQ__Status__737584F65676DE1C")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("Trip");

                entity.Property(e => e.Distance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Trips)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK__Trip__VehicleId__398D8EEE");
            });

            modelBuilder.Entity<Truck>(entity =>
            {
                entity.ToTable("Truck");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Trucks)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK__Truck__VehicleId__36B12243");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicle");

                entity.HasIndex(e => e.LicensePlate, "UQ__Vehicle__026BC15C360365A4")
                    .IsUnique();

                entity.Property(e => e.AverageSpeed).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.DistanceDriven).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DistanceReversed).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.LicensePlate)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__Vehicle__StatusI__2D27B809");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
