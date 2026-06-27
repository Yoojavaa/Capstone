using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace capsproject.Models
{
    public class FloodPolygon
    {
        public Guid PolygonId { get; set; } = Guid.NewGuid();
        public Guid NodeId { get; set; }

        // Stores the actual 2D boundary of the flood for Leaflet/OSRM
        public Polygon ZoneGeometry { get; set; }

        public int DangerLevel { get; set; } // 1 = Passable, 2 = High Clearance, 3 = Impassable
        public bool IsCurrentlyFlooded { get; set; }
        public DateTime LastStatusChange { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public SensorNode SensorNode { get; set; }
    }
}
