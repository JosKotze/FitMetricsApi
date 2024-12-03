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
                    SegmentEfforts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SplitMetric = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Laps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KudosCount = table.Column<int>(type: "int", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    AchievementCount = table.Column<int>(type: "int", nullable: false),
                    AthleteCount = table.Column<int>(type: "int", nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    activityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SummaryPolyline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
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
                    AverageHeartrate = table.Column<double>(type: "float", nullable: true),
                    AverageSpeed = table.Column<double>(type: "float", nullable: false),
                    TotalElevationGain = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDateLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxHeartrate = table.Column<double>(type: "float", nullable: true),
                    ActivityDetailsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityDetails_ActivityDetailsId",
                        column: x => x.ActivityDetailsId,
                        principalTable: "ActivityDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityDetailsId",
                table: "Activities",
                column: "ActivityDetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Athletes");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "ActivityDetails");
        }
    }
}
