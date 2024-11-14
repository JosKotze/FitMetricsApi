using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strava2ExcelWebApiBackend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMapJsonProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Map_MapId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "Map");

            migrationBuilder.DropIndex(
                name: "IX_Activities_MapId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "Map",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Map",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "MapId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Map",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResourceState = table.Column<int>(type: "int", nullable: false),
                    SummaryPolyline = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MapId",
                table: "Activities",
                column: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Map_MapId",
                table: "Activities",
                column: "MapId",
                principalTable: "Map",
                principalColumn: "Id");
        }
    }
}
