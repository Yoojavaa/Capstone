using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace capsproject.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensorNodes",
                columns: table => new
                {
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coordinates = table.Column<Point>(type: "geography", nullable: false),
                    BaseElevationMeter = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorNodes", x => x.NodeId);
                });

            migrationBuilder.CreateTable(
                name: "FloodPolygons",
                columns: table => new
                {
                    PolygonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZoneGeometry = table.Column<Polygon>(type: "geography", nullable: false),
                    DangerLevel = table.Column<int>(type: "int", nullable: false),
                    IsCurrentlyFlooded = table.Column<bool>(type: "bit", nullable: false),
                    LastStatusChange = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodPolygons", x => x.PolygonId);
                    table.ForeignKey(
                        name: "FK_FloodPolygons_SensorNodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "SensorNodes",
                        principalColumn: "NodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloodTelemetries",
                columns: table => new
                {
                    TelemetryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WaterDepthCM = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloodTelemetries", x => x.TelemetryId);
                    table.ForeignKey(
                        name: "FK_FloodTelemetries_SensorNodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "SensorNodes",
                        principalColumn: "NodeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloodPolygons_NodeId",
                table: "FloodPolygons",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_FloodTelemetries_NodeId",
                table: "FloodTelemetries",
                column: "NodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloodPolygons");

            migrationBuilder.DropTable(
                name: "FloodTelemetries");

            migrationBuilder.DropTable(
                name: "SensorNodes");
        }
    }
}
