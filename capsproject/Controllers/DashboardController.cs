using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DriveWise.Models;

namespace DriveWise.Controllers
{
    public class DashboardController : Controller
    {
        // GET: /Dashboard  or  /Dashboard/Index
        public ActionResult Index()
        {
            var model = BuildMockDashboard();
            return View(model);
        }

        // GET: /Dashboard/Map
        public ActionResult Map()
        {
            var model = BuildMockDashboard();
            ViewBag.Title = "Live Map";
            return View(model);
        }

        // GET: /Dashboard/Sensors
        public ActionResult Sensors()
        {
            var model = BuildMockDashboard();
            ViewBag.Title = "Sensors";
            return View(model);
        }

        // GET: /Dashboard/Alerts
        public ActionResult Alerts()
        {
            var model = BuildMockDashboard();
            ViewBag.Title = "Alerts";
            return View(model);
        }

        // GET: /Dashboard/RoutePlanner
        public ActionResult RoutePlanner()
        {
            var model = BuildMockDashboard();
            ViewBag.Title = "Route Planner";
            return View(model);
        }

        // GET: /Dashboard/Reports
        public ActionResult Reports()
        {
            var model = BuildMockDashboard();
            ViewBag.Title = "Reports";
            return View(model);
        }

        // GET: /Dashboard/Logs
        public ActionResult Logs()
        {
            var model = BuildMockDashboard();
            ViewBag.Title = "Logs";
            return View(model);
        }

        // GET: /Dashboard/Users
        public ActionResult Users()
        {
            var model = BuildMockDashboard();
            ViewBag.Title = "Users";
            return View(model);
        }

        // GET: /Dashboard/Settings
        public ActionResult Settings()
        {
            var model = BuildMockDashboard();
            ViewBag.Title = "Settings";
            return View(model);
        }

        // GET: /Dashboard/Refresh
        // Lightweight endpoint the front-end can poll (e.g. every 30s) to refresh
        // sensor cards / map / alerts without a full page reload.
        [HttpGet]
        public JsonResult Refresh()
        {
            var model = BuildMockDashboard();
            return Json(new
            {
                totalSensors = model.TotalSensors,
                onlineSensors = model.OnlineSensors,
                offlineSensors = model.OfflineSensors,
                normalCount = model.NormalCount,
                warningCount = model.WarningCount,
                dangerCount = model.DangerCount,
                rainfallTodayMm = model.RainfallTodayMm,
                sensors = model.Sensors.Select(s => new
                {
                    code = s.Code,
                    location = s.Location,
                    waterLevelCm = s.WaterLevelCm,
                    status = s.Status.ToString(),
                    updatedAt = s.UpdatedAt.ToString("hh:mm tt"),
                    lat = s.Lat,
                    lng = s.Lng
                }),
                alerts = model.RecentAlerts.Select(a => new
                {
                    location = a.Location,
                    waterLevelCm = a.WaterLevelCm,
                    status = a.Status.ToString(),
                    raisedAt = a.RaisedAt.ToString("hh:mm tt")
                })
            });
        }

        // ---------------------------------------------------------------
        // Replace this with real data: EF Core/Dapper query against your
        // sensors table, a call to your telemetry API, etc.
        // ---------------------------------------------------------------
        private DashboardViewModel BuildMockDashboard()
        {
            var now = DateTime.Now;

            var sensors = new List<SensorReading>
            {
                new SensorReading { Code = "S01", Location = "Davao River - Matina",   WaterLevelCm = 82, Status = SensorStatus.Danger,  UpdatedAt = now.AddMinutes(-1),  Lat = 38, Lng = 28 },
                new SensorReading { Code = "S02", Location = "Buhangin Bridge",         WaterLevelCm = 55, Status = SensorStatus.Warning, UpdatedAt = now.AddMinutes(-2),  Lat = 22, Lng = 44 },
                new SensorReading { Code = "S03", Location = "Ecoland - Quimpo Blvd",   WaterLevelCm = 48, Status = SensorStatus.Warning, UpdatedAt = now.AddMinutes(-2),  Lat = 52, Lng = 50 },
                new SensorReading { Code = "S04", Location = "Roxas Ave - Bangkal",     WaterLevelCm = 18, Status = SensorStatus.Normal,  UpdatedAt = now.AddMinutes(-3),  Lat = 60, Lng = 62 },
                new SensorReading { Code = "S05", Location = "Toril - Diversion Rd",    WaterLevelCm = 22, Status = SensorStatus.Normal,  UpdatedAt = now.AddMinutes(-4),  Lat = 60, Lng = 24 },
                new SensorReading { Code = "S06", Location = "Talomo District",         WaterLevelCm = 16, Status = SensorStatus.Normal,  UpdatedAt = now.AddMinutes(-5),  Lat = 20, Lng = 19 },
                new SensorReading { Code = "S07", Location = "Bago Oshiro",             WaterLevelCm = 71, Status = SensorStatus.Danger,  UpdatedAt = now.AddMinutes(-6),  Lat = 16, Lng = 70 },
                new SensorReading { Code = "S08", Location = "Bangrat",                 WaterLevelCm = 70, Status = SensorStatus.Danger,  UpdatedAt = now.AddMinutes(-7),  Lat = 70, Lng = 36 },
                new SensorReading { Code = "S09", Location = "Matina Aplaya",           WaterLevelCm = 35, Status = SensorStatus.Warning, UpdatedAt = now.AddMinutes(-8),  Lat = 65, Lng = 80 },
            };

            var alerts = new List<AlertItem>
            {
                new AlertItem { Location = "Davao River - Matina",  WaterLevelCm = 82, Status = SensorStatus.Danger,  RaisedAt = now.AddMinutes(-5) },
                new AlertItem { Location = "Buhangin Bridge",       WaterLevelCm = 55, Status = SensorStatus.Warning, RaisedAt = now.AddMinutes(-9) },
                new AlertItem { Location = "Ecoland - Quimpo Blvd", WaterLevelCm = 48, Status = SensorStatus.Warning, RaisedAt = now.AddMinutes(-12) },
                new AlertItem { Location = "Roxas Ave - Bangkal",   WaterLevelCm = 18, Status = SensorStatus.Normal,  RaisedAt = now.AddMinutes(-19) },
                new AlertItem { Location = "Toril - Diversion Rd",  WaterLevelCm = 22, Status = SensorStatus.Normal,  RaisedAt = now.AddMinutes(-24) },
            };

            var history = new List<WaterLevelPoint>
            {
                new WaterLevelPoint { TimeLabel = "12 AM", LevelCm = 20 },
                new WaterLevelPoint { TimeLabel = "3 AM",  LevelCm = 18 },
                new WaterLevelPoint { TimeLabel = "6 AM",  LevelCm = 24 },
                new WaterLevelPoint { TimeLabel = "9 AM",  LevelCm = 38 },
                new WaterLevelPoint { TimeLabel = "12 PM", LevelCm = 78 },
                new WaterLevelPoint { TimeLabel = "3 PM",  LevelCm = 82 },
                new WaterLevelPoint { TimeLabel = "6 PM",  LevelCm = 60 },
                new WaterLevelPoint { TimeLabel = "9 PM",  LevelCm = 40 },
            };

            var model = new DashboardViewModel
            {
                CityName = "Davao City",
                ServerTime = now,
                AllSystemsOnline = true,
                Sensors = sensors,
                RecentAlerts = alerts,
                WaterLevelHistory = history,
                TotalSensors = sensors.Count,
                OnlineSensors = sensors.Count(s => s.Online),
                OfflineSensors = sensors.Count(s => !s.Online),
                NormalCount = sensors.Count(s => s.Status == SensorStatus.Normal),
                WarningCount = sensors.Count(s => s.Status == SensorStatus.Warning),
                DangerCount = sensors.Count(s => s.Status == SensorStatus.Danger),
                RainfallTodayMm = 42.6,
                RainfallDescription = "Moderate Rain",
                Weather = new WeatherInfo
                {
                    TemperatureC = 26,
                    Condition = "Moderate Rain",
                    HumidityPercent = 89,
                    WindKmh = 12,
                    RainTodayMm = 42.6,
                    Forecast = "Heavy Rain"
                },
                UnreadAlerts = alerts.Count
            };

            return model;
        }
    }
}
