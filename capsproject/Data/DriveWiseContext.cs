using Microsoft.EntityFrameworkCore;
using capsproject.Models; // Tells the context where to find your models

namespace capsproject.Data // Matches your project name
{
    public class DriveWiseContext : DbContext // Matches your file name
    {
        public DriveWiseContext(DbContextOptions<DriveWiseContext> options) : base(options)
        {
        }

        public DbSet<SensorNode> SensorNodes { get; set; }
        public DbSet<FloodTelemetry> FloodTelemetries { get; set; }
        public DbSet<FloodPolygon> FloodPolygons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configure SensorNode
            modelBuilder.Entity<SensorNode>()
                .HasKey(s => s.NodeId);
            modelBuilder.Entity<SensorNode>()
                .Property(s => s.BaseElevationMeter)
                .HasColumnType("decimal(5,2)");

            // 2. Configure FloodTelemetry
            modelBuilder.Entity<FloodTelemetry>()
                .HasKey(t => t.TelemetryId);
            modelBuilder.Entity<FloodTelemetry>()
                .Property(t => t.WaterDepthCM)
                .HasColumnType("decimal(5,2)");

            // Relationship: One SensorNode has many Telemetry readings
            modelBuilder.Entity<FloodTelemetry>()
                .HasOne(t => t.SensorNode)
                .WithMany(s => s.TelemetryReadings)
                .HasForeignKey(t => t.NodeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Configure FloodPolygon
            modelBuilder.Entity<FloodPolygon>()
                .HasKey(p => p.PolygonId);

            // Relationship: One SensorNode triggers a FloodPolygon
            modelBuilder.Entity<FloodPolygon>()
                .HasOne(p => p.SensorNode)
                .WithMany(s => s.FloodZones)
                .HasForeignKey(p => p.NodeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}