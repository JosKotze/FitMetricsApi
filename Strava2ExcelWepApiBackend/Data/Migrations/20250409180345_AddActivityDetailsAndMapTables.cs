using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strava2ExcelWebApiBackend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityDetailsAndMapTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Laps",
                table: "ActivityDetails");

            migrationBuilder.AlterColumn<int>(
                name: "WeightedAverageWatts",
                table: "ActivityDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Trainer",
                table: "ActivityDetails",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StartLatlng",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SportType",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxWatts",
                table: "ActivityDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KudosCount",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EndLatlng",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmbedToken",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceName",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AthleteCount",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AchievementCount",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AthleteId",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "AverageCadence",
                table: "ActivityDetails",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageSpeed",
                table: "ActivityDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Commute",
                table: "ActivityDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "ActivityDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ElapsedTime",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Flagged",
                table: "ActivityDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GearId",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasKudoed",
                table: "ActivityDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LapsJson",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Manual",
                table: "ActivityDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MapId",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "MaxSpeed",
                table: "ActivityDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MovingTime",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PhotoCount",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Polyline",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PrCount",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Private",
                table: "ActivityDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ResourceState",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SegmentEffortsJson",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SplitsMetricJson",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ActivityDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateLocal",
                table: "ActivityDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SummaryPolyline",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TotalElevationGain",
                table: "ActivityDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPhotoCount",
                table: "ActivityDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UploadId",
                table: "ActivityDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UtcOffset",
                table: "ActivityDetails",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Visibility",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "WorkoutType",
                table: "ActivityDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AthleteId",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "AverageCadence",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "AverageSpeed",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Commute",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "ElapsedTime",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Flagged",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "GearId",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "HasKudoed",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "LapsJson",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Manual",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "MaxSpeed",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "MovingTime",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "PhotoCount",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Polyline",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "PrCount",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Private",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "ResourceState",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "SegmentEffortsJson",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "SplitsMetricJson",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "StartDateLocal",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "SummaryPolyline",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "TotalElevationGain",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "TotalPhotoCount",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "UploadId",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "UtcOffset",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "ActivityDetails");

            migrationBuilder.DropColumn(
                name: "WorkoutType",
                table: "ActivityDetails");

            migrationBuilder.AlterColumn<double>(
                name: "WeightedAverageWatts",
                table: "ActivityDetails",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Trainer",
                table: "ActivityDetails",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "StartLatlng",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SportType",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "MaxWatts",
                table: "ActivityDetails",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KudosCount",
                table: "ActivityDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EndLatlng",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmbedToken",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceName",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "ActivityDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AthleteCount",
                table: "ActivityDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AchievementCount",
                table: "ActivityDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Laps",
                table: "ActivityDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
