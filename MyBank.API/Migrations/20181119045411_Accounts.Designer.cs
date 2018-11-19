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
    [Migration("20181119045411_Accounts")]
    partial class Accounts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview3-35497");

            modelBuilder.Entity("MyBank.API.Models.Account", b =>
                {
                    b.Property<int>("accountID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("accountName");

                    b.Property<decimal>("accountPercent");

                    b.Property<decimal>("accountTotal");

                    b.Property<int>("userID");

                    b.HasKey("accountID");

                    b.HasIndex("userID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MyBank.API.Models.TransactionHistory", b =>
                {
                    b.Property<int>("transactionHistoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("accountID");

                    b.Property<DateTime>("transactionDateTime");

                    b.Property<string>("transactionType");

                    b.HasKey("transactionHistoryID");

                    b.HasIndex("accountID");

                    b.ToTable("TransactionHistories");
                });

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

            modelBuilder.Entity("MyBank.API.Models.Account", b =>
                {
                    b.HasOne("MyBank.API.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("userID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyBank.API.Models.TransactionHistory", b =>
                {
                    b.HasOne("MyBank.API.Models.Account", "accounts")
                        .WithMany("TransactionHistory")
                        .HasForeignKey("accountID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
