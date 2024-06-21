using System;
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
            migrationBuilder.DropTable(
                name: "UserAuthentication");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "Athletes",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Athletes",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Athletes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Athletes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Athletes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreashTokenCreated",
                table: "Athletes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Athletes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "RefreashTokenCreated",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Athletes");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Athletes",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Athletes",
                newName: "name");

            migrationBuilder.CreateTable(
                name: "UserAuthentication",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    accessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    refreashTokenCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    refreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthentication", x => x.Id);
                });
        }
    }
}
