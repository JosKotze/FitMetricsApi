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
    [Migration("20241203190920_InitialCreate")]
    partial class InitialCreate
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

                    b.Property<long>("ActivityId")
                        .HasColumnType("bigint");

                    b.Property<double?>("AverageHeartrate")
                        .HasColumnType("float");

                    b.Property<double>("AverageSpeed")
                        .HasColumnType("float");

                    b.Property<double>("Distance")
                        .HasColumnType("float");

                    b.Property<double?>("MaxHeartrate")
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

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Strava2ExcelWebApiBackend.Models.ActivityDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AchievementCount")
                        .HasColumnType("int");

                    b.Property<long>("ActivityId")
                        .HasColumnType("bigint");

                    b.Property<int>("AthleteCount")
                        .HasColumnType("int");

                    b.Property<double?>("AverageWatts")
                        .HasColumnType("float");

                    b.Property<double?>("Calories")
                        .HasColumnType("float");

                    b.Property<int>("CommentCount")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("DeviceWatts")
                        .HasColumnType("bit");

                    b.Property<double?>("ElevHigh")
                        .HasColumnType("float");

                    b.Property<double?>("ElevLow")
                        .HasColumnType("float");

                    b.Property<string>("EmbedToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndLatlng")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Kilojoules")
                        .HasColumnType("float");

                    b.Property<int>("KudosCount")
                        .HasColumnType("int");

                    b.Property<string>("Laps")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("MaxWatts")
                        .HasColumnType("float");

                    b.Property<string>("SegmentEfforts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SplitMetric")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SportType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StartLatlng")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Trainer")
                        .HasColumnType("bit");

                    b.Property<double?>("WeightedAverageWatts")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ActivityDetails");
                });

            modelBuilder.Entity("Strava2ExcelWebApiBackend.Models.Map", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("ActivityId")
                        .HasColumnType("bigint");

                    b.Property<string>("Polyline")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Maps");
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