using System;
using System.Collections.Generic;
using StatlerWaldorfCorp.LocationService.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
    public class LocationDbContext: DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }

        public DbSet<LocationRecord> LocationRecords { get; set;}        
    }

    public class LocationDbContextFactory : IDesignTimeDbContextFactory<LocationDbContext>
    {

        public LocationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocationDbContext>();
            var connectionString = "Host=localhost;Port=5432;Database=locationservice;Username=postgres;Password=mysecretpassword";
            optionsBuilder.UseNpgsql(connectionString);
            return new LocationDbContext(optionsBuilder.Options);
        }
    }
}