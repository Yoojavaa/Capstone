using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace capsproject.Models
{
    public class SensorNode
    {
        public Guid NodeId { get; set; } = Guid.NewGuid();
        public string LocationName { get; set; } = string.Empty;

        // Stores the exact GPS location of the hardware
        public Point Coordinates { get; set; }

        public decimal BaseElevationMeter { get; set; }
        public bool IsActive { get; set; }

        // Navigation Properties for relationships
        public ICollection<FloodTelemetry> TelemetryReadings { get; set; } = new List<FloodTelemetry>();
        public ICollection<FloodPolygon> FloodZones { get; set; } = new List<FloodPolygon>();
    }
}
