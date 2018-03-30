﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using User.API.Data;

namespace User.API.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("User.API.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar")
                        .HasMaxLength(256);

                    b.Property<string>("Company")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Phone");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("sysAppUsers");
                });

            modelBuilder.Entity("User.API.Models.UserProperty", b =>
                {
                    b.Property<int>("AppUserId");

                    b.Property<string>("Value")
                        .HasMaxLength(50);

                    b.Property<string>("Key")
                        .HasMaxLength(50);

                    b.Property<string>("Text")
                        .HasMaxLength(128);

                    b.HasKey("AppUserId", "Value", "Key");

                    b.ToTable("tbUserProperties");
                });

            modelBuilder.Entity("User.API.Models.UserProperty", b =>
                {
                    b.HasOne("User.API.Models.AppUser")
                        .WithMany("UserProperties")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
