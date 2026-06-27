using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace capsproject.Models
{
    public class FloodTelemetry
    {
        public long TelemetryId { get; set; }
        public Guid NodeId { get; set; }
        public decimal WaterDepthCM { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public SensorNode SensorNode { get; set; }
    }
}
