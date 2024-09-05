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
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    summary_polyline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    resource_state = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceState = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    MovingTime = table.Column<int>(type: "int", nullable: false),
                    ElapsedTime = table.Column<int>(type: "int", nullable: false),
                    TotalElevationGain = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SportType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkoutType = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDateLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtcOffset = table.Column<double>(type: "float", nullable: true),
                    LocationCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AchievementCount = table.Column<int>(type: "int", nullable: true),
                    KudosCount = table.Column<int>(type: "int", nullable: true),
                    CommentCount = table.Column<int>(type: "int", nullable: true),
                    AthleteCount = table.Column<int>(type: "int", nullable: true),
                    PhotoCount = table.Column<int>(type: "int", nullable: true),
                    Mapid = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    HasHeartrate = table.Column<bool>(type: "bit", nullable: false),
                    HeartrateOptOut = table.Column<bool>(type: "bit", nullable: false),
                    DisplayHideHeartrateOption = table.Column<bool>(type: "bit", nullable: false),
                    UploadId = table.Column<long>(type: "bigint", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromAcceptedTag = table.Column<bool>(type: "bit", nullable: false),
                    PrCount = table.Column<int>(type: "int", nullable: false),
                    TotalPhotoCount = table.Column<int>(type: "int", nullable: false),
                    HasKudoed = table.Column<bool>(type: "bit", nullable: false),
                    AverageWatts = table.Column<double>(type: "float", nullable: true),
                    Kilojoules = table.Column<double>(type: "float", nullable: true),
                    DeviceWatts = table.Column<bool>(type: "bit", nullable: true),
                    AverageHeartrate = table.Column<double>(type: "float", nullable: true),
                    MaxHeartrate = table.Column<double>(type: "float", nullable: true),
                    ElevHigh = table.Column<double>(type: "float", nullable: true),
                    ElevLow = table.Column<double>(type: "float", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Athletes_UserId",
                        column: x => x.UserId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_Map_Mapid",
                        column: x => x.Mapid,
                        principalTable: "Map",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Mapid",
                table: "Activities",
                column: "Mapid");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId",
                table: "Activities",
                column: "UserId");
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
