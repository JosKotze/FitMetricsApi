﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Strava2ExcelWebApiBackend.Data;

#nullable disable

namespace Strava2ExcelWebApiBackend.Data.Migrations
{
    [DbContext(typeof(StravaDbContext))]
    [Migration("20240905191834_RemovedOldModels")]
    partial class RemovedOldModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Strava2ExcelWebApiBackend.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AchievementCount")
                        .HasColumnType("int");

                    b.Property<double?>("AverageHeartrate")
                        .HasColumnType("float");

                    b.Property<double>("AverageSpeed")
                        .HasColumnType("float");

                    b.Property<double?>("AverageWatts")
                        .HasColumnType("float");

                    b.Property<bool?>("DeviceWatts")
                        .HasColumnType("bit");

                    b.Property<double>("Distance")
                        .HasColumnType("float");

                    b.Property<double?>("Kilojoules")
                        .HasColumnType("float");

                    b.Property<string>("LocationCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocationCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocationState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("MaxHeartrate")
                        .HasColumnType("float");

                    b.Property<double>("MaxSpeed")
                        .HasColumnType("float");

                    b.Property<int>("MovingTime")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartDateLocal")
                        .HasColumnType("datetime2");

                    b.Property<string>("Timezone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalElevationGain")
                        .HasColumnType("float");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<double?>("UtcOffset")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Strava2ExcelWebApiBackend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Athletes");
                });
#pragma warning restore 612, 618
        }
    }
}
