using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strava2ExcelWebApiBackend.Data.Migrations
{
    /// <inheritdoc />
    public partial class SqlInitial : Migration
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
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Pace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    MovingTime = table.Column<int>(type: "int", nullable: false),
                    AverageHeartrate = table.Column<double>(type: "float", nullable: true),
                    AverageSpeed = table.Column<double>(type: "float", nullable: false),
                    TotalElevationGain = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDateLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxHeartrate = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Calories = table.Column<double>(type: "float", nullable: true),
                    Trainer = table.Column<bool>(type: "bit", nullable: true),
                    AverageWatts = table.Column<double>(type: "float", nullable: true),
                    MaxWatts = table.Column<double>(type: "float", nullable: true),
                    WeightedAverageWatts = table.Column<double>(type: "float", nullable: true),
                    Kilojoules = table.Column<double>(type: "float", nullable: true),
                    DeviceWatts = table.Column<bool>(type: "bit", nullable: true),
                    ElevHigh = table.Column<double>(type: "float", nullable: true),
                    ElevLow = table.Column<double>(type: "float", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbedToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Laps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KudosCount = table.Column<int>(type: "int", nullable: true),
                    CommentCount = table.Column<int>(type: "int", nullable: true),
                    AchievementCount = table.Column<int>(type: "int", nullable: true),
                    AthleteCount = table.Column<int>(type: "int", nullable: true),
                    SportType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartLatlng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndLatlng = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDetails", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Polyline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartLatlng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndLatlng = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ActivityDetails");

            migrationBuilder.DropTable(
                name: "Athletes");

            migrationBuilder.DropTable(
                name: "Maps");
        }
    }
}
