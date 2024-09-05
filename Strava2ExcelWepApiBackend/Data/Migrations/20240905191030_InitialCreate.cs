using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strava2ExcelWebApiBackend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    MovingTime = table.Column<int>(type: "int", nullable: false),
                    TotalElevationGain = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDateLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtcOffset = table.Column<double>(type: "float", nullable: true),
                    LocationCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievementCount = table.Column<int>(type: "int", nullable: true),
                    AverageSpeed = table.Column<double>(type: "float", nullable: false),
                    MaxSpeed = table.Column<double>(type: "float", nullable: false),
                    AverageWatts = table.Column<double>(type: "float", nullable: true),
                    Kilojoules = table.Column<double>(type: "float", nullable: true),
                    DeviceWatts = table.Column<bool>(type: "bit", nullable: true),
                    AverageHeartrate = table.Column<double>(type: "float", nullable: true),
                    MaxHeartrate = table.Column<double>(type: "float", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Athletes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Athletes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Athletes");
            migrationBuilder.DropTable(
                name: "Map");
        }
    }
}
