using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strava2ExcelWebApiBackend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityDetailsAndMapTablesFour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageHeartrate",
                table: "ActivityDetails",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxHeartrate",
                table: "ActivityDetails",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageHeartrate",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "MaxHeartrate",
                table: "ActivityDetails");
        }
    }
}
