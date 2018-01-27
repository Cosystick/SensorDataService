﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SensorService.API.Models;
using System;

namespace SensorService.API.Migrations
{
    [DbContext(typeof(SensorContext))]
    partial class SensorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("SensorService.API.Models.Device", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsVisible");

                    b.Property<string>("Name");

                    b.Property<int>("CurrentUserId");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("SensorService.API.Models.Sensor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DeviceId");

                    b.Property<string>("SensorKey");

                    b.Property<int>("SensorType");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("Sensor");
                });

            modelBuilder.Entity("SensorService.API.Models.SensorData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<int?>("SensorId");

                    b.Property<long>("Value");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("SensorData");
                });

            modelBuilder.Entity("SensorService.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<bool>("IsAdministrator");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SensorService.API.Models.Sensor", b =>
                {
                    b.HasOne("SensorService.API.Models.Device")
                        .WithMany("Sensors")
                        .HasForeignKey("DeviceId");
                });

            modelBuilder.Entity("SensorService.API.Models.SensorData", b =>
                {
                    b.HasOne("SensorService.API.Models.Sensor")
                        .WithMany("Data")
                        .HasForeignKey("SensorId");
                });
#pragma warning restore 612, 618
        }
    }
}
