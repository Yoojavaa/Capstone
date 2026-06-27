using System;
using System.Collections.Generic;

namespace DriveWise.Models
{
    public enum SensorStatus
    {
        Normal,
        Warning,
        Danger
    }

    public class SensorReading
    {
        public string Code { get; set; }          // "S01"
        public string Location { get; set; }       // "Davao River - Matina"
        public double WaterLevelCm { get; set; }    // 82
        public SensorStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double Lat { get; set; }             // map position (percent or real coords)
        public double Lng { get; set; }
        public bool Online { get; set; } = true;
    }

    public class AlertItem
    {
        public string Location { get; set; }
        public double WaterLevelCm { get; set; }
        public SensorStatus Status { get; set; }
        public DateTime RaisedAt { get; set; }
    }

    public class WaterLevelPoint
    {
        public string TimeLabel { get; set; }   // "12 AM", "3 AM" ...
        public double LevelCm { get; set; }
    }

    public class WeatherInfo
    {
        public double TemperatureC { get; set; }
        public string Condition { get; set; }
        public int HumidityPercent { get; set; }
        public double WindKmh { get; set; }
        public double RainTodayMm { get; set; }
        public string Forecast { get; set; }
    }

    public class DashboardViewModel
    {
        public string CityName { get; set; } = "Davao City";
        public DateTime ServerTime { get; set; } = DateTime.Now;
        public bool AllSystemsOnline { get; set; } = true;
        public int UnreadAlerts { get; set; }

        public int TotalSensors { get; set; }
        public int OnlineSensors { get; set; }
        public int OfflineSensors { get; set; }

        public int NormalCount { get; set; }
        public int WarningCount { get; set; }
        public int DangerCount { get; set; }

        public double RainfallTodayMm { get; set; }
        public string RainfallDescription { get; set; }

        public List<SensorReading> Sensors { get; set; } = new List<SensorReading>();
        public List<AlertItem> RecentAlerts { get; set; } = new List<AlertItem>();
        public List<WaterLevelPoint> WaterLevelHistory { get; set; } = new List<WaterLevelPoint>();
        public WeatherInfo Weather { get; set; } = new WeatherInfo();

        public double NormalThresholdCm { get; set; } = 30;
        public double WarningThresholdCm { get; set; } = 70;

        public double NormalPercent => TotalSensors == 0 ? 0 : Math.Round((double)NormalCount / TotalSensors * 100, 1);
        public double WarningPercent => TotalSensors == 0 ? 0 : Math.Round((double)WarningCount / TotalSensors * 100, 1);
        public double DangerPercent => TotalSensors == 0 ? 0 : Math.Round((double)DangerCount / TotalSensors * 100, 1);
    }
}
