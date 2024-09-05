using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strava2ExcelWebApiBackend.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resource_state",
                table: "Athletes");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "resource_state",
                table: "Athletes",
                type: "nvarchar(max)",
                nullable: true);
        }

    }
}
