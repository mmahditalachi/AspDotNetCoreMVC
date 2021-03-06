﻿// <auto-generated />
using System;
using Dashboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dashboard.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200129073207_InitialMigratin")]
    partial class InitialMigratin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dashboard.Models.Users", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ConfrimPassword")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)")
                        .HasMaxLength(11);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Password")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)")
                        .HasMaxLength(11);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("UserID");

                    b.ToTable("_User");

                    b.HasData(
                        new
                        {
                            UserID = new Guid("11a8e2a8-aeb6-4c22-90bc-9d7b9a5e07bd"),
                            ConfrimPassword = new Guid("4bf44e48-7066-483e-ba40-8be7141c5be7"),
                            Email = "test3@test.com",
                            FirstName = "mohammad",
                            HomeNumber = "22771209",
                            LastName = "talachi",
                            Password = new Guid("4bf44e48-7066-483e-ba40-8be7141c5be7"),
                            PhoneNumber = "09126344398",
                            Username = "king_mohi"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
