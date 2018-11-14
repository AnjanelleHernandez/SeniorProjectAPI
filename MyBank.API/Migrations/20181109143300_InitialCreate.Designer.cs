﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBank.API.Data;

namespace MyBank.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20181109143300_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview3-35497");

            modelBuilder.Entity("MyBank.API.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("emailAddress");

                    b.Property<string>("firstName");

                    b.Property<string>("lastName");

                    b.Property<byte[]>("passwordHash");

                    b.Property<byte[]>("passwordSalt");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyBank.API.Models.Value", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Values");
                });
#pragma warning restore 612, 618
        }
    }
}
