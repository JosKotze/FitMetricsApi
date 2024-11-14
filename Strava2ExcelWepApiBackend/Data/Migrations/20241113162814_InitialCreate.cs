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
                name: "Map",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SummaryPolyline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                });

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
                    ElapsedTime = table.Column<int>(type: "int", nullable: false),
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
                    KudosCount = table.Column<int>(type: "int", nullable: true),
                    Commentount = table.Column<int>(type: "int", nullable: true),
                    AthleteCount = table.Column<int>(type: "int", nullable: true),
                    PhotoCount = table.Column<int>(type: "int", nullable: true),
                    Trainer = table.Column<bool>(type: "bit", nullable: true),
                    Commute = table.Column<bool>(type: "bit", nullable: true),
                    Manual = table.Column<bool>(type: "bit", nullable: true),
                    Private = table.Column<bool>(type: "bit", nullable: true),
                    Visibility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flagged = table.Column<bool>(type: "bit", nullable: true),
                    GearId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartLatlng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndLatlng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageSpeed = table.Column<double>(type: "float", nullable: false),
                    MaxSpeed = table.Column<double>(type: "float", nullable: false),
                    AverageWatts = table.Column<double>(type: "float", nullable: true),
                    MaxWatts = table.Column<double>(type: "float", nullable: true),
                    WeightedAverageWatts = table.Column<double>(type: "float", nullable: true),
                    Kilojoules = table.Column<double>(type: "float", nullable: true),
                    DeviceWatts = table.Column<bool>(type: "bit", nullable: true),
                    HasHeartrate = table.Column<bool>(type: "bit", nullable: false),
                    AverageHeartrate = table.Column<double>(type: "float", nullable: true),
                    MaxHeartrate = table.Column<double>(type: "float", nullable: true),
                    ElevHigh = table.Column<double>(type: "float", nullable: true),
                    ElevLow = table.Column<double>(type: "float", nullable: true),
                    PrCount = table.Column<int>(type: "int", nullable: false),
                    UploadId = table.Column<long>(type: "bigint", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPhotoCount = table.Column<int>(type: "int", nullable: false),
                    HasKudoed = table.Column<bool>(type: "bit", nullable: false),
                    MapId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Map_MapId",
                        column: x => x.MapId,
                        principalTable: "Map",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MapId",
                table: "Activities",
                column: "MapId");
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
