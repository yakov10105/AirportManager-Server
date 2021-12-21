using Final_Project.Server.DAL.Extentions;
using Final_Project.Server.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.DAL
{
    public class AirportDbContext : DbContext
    {
        public AirportDbContext(DbContextOptions<AirportDbContext> options) : base(options) 
        {
        
        }

        public virtual DbSet<Airplane> Airplanes { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<FlightHistory> FlightHistories { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<ControlTower> ControlTowers { get; set; }
        public virtual DbSet<StationToStation> StationsToStations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControlTower>()
                .HasIndex(ct => ct.Name)
                .IsUnique();

            modelBuilder.Entity<Station>()
               .HasOne(s => s.CurrentFlight)
               .WithOne(f => f.Station)
               .HasForeignKey<Station>(s => s.CurrentFlightId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StationToStation>(entity =>
            {
                entity.HasKey(sts => new { sts.StationToId, sts.StationFromId, sts.Direction });
                entity
                   .HasOne(sr => sr.FromStation)
                   .WithMany(s => s.ChildrenStations)
                   .HasForeignKey(sr => sr.StationFromId)
                   .OnDelete(DeleteBehavior.NoAction);
                entity
                    .HasOne(sr => sr.ToStation)
                    .WithMany(s => s.ParentStations)
                    .HasForeignKey(sr => sr.StationToId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StationToControlTower>(entity =>
            {
                entity.HasKey(sct => new { sct.StationToId, sct.Direction, sct.ControlTowerId });
                entity
                    .HasOne(sct=>sct.Station)
                    .WithOne(s=>s.ControlTowerRelation)
                    .HasForeignKey<StationToControlTower>(sct=>sct.StationToId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity
                    .HasOne(sct => sct.ControlTower)
                    .WithMany(ct => ct.ConnectedStations)
                    .HasForeignKey(sct => sct.ControlTowerId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<FlightHistory>()
                .HasOne(fh => fh.Flight)
                .WithMany(f => f.History);


            modelBuilder.SeedDataOnCreate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }

    
}
