﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UsersApi;

namespace UsersApi.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    [Migration("20210508202931_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("UsersApi.DataBases.UserDbo", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("UserId");

                    b.HasIndex(new[] { "Name" }, "Idx_UserName")
                        .IsUnique();

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
