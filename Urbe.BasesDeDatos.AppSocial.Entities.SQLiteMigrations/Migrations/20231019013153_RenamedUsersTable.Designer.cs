﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.BasesDeDatos.AppSocial.Entities;

#nullable disable

namespace Urbe.BasesDeDatos.AppSocial.Entities.SQLiteMigrations.Migrations
{
    [DbContext(typeof(SocialContext))]
    [Migration("20231019013153_RenamedUsersTable")]
    partial class RenamedUsersTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SocialAppUserClaims", (string)null);
                });

            modelBuilder.Entity("SocialAppUserFollow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FollowedId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FollowedId");

                    b.HasIndex("FollowerId");

                    b.ToTable("SocialAppUserFollow");
                });

            modelBuilder.Entity("Urbe.BasesDeDatos.AppSocial.Entities.Models.PendingMailConfirmation", b =>
                {
                    b.Property<byte[]>("Id")
                        .HasColumnType("BLOB");

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PendingMailConfirmations");
                });

            modelBuilder.Entity("Urbe.BasesDeDatos.AppSocial.Entities.Models.Post", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("DatePosted")
                        .HasColumnType("TEXT");

                    b.Property<long>("InResponseToId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PosterId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PosterThenUsername")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("InResponseToId");

                    b.HasIndex("PosterId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Urbe.BasesDeDatos.AppSocial.Entities.Models.SocialAppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FollowerCount")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProfileMessage")
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfilePictureUrl")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<string>("Pronouns")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("RealName")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("Settings")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(30ul);

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("SocialAppUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Urbe.BasesDeDatos.AppSocial.Entities.Models.SocialAppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SocialAppUserFollow", b =>
                {
                    b.HasOne("Urbe.BasesDeDatos.AppSocial.Entities.Models.SocialAppUser", "Followed")
                        .WithMany()
                        .HasForeignKey("FollowedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Urbe.BasesDeDatos.AppSocial.Entities.Models.SocialAppUser", "Follower")
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Followed");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("Urbe.BasesDeDatos.AppSocial.Entities.Models.PendingMailConfirmation", b =>
                {
                    b.HasOne("Urbe.BasesDeDatos.AppSocial.Entities.Models.SocialAppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Urbe.BasesDeDatos.AppSocial.Entities.Models.Post", b =>
                {
                    b.HasOne("Urbe.BasesDeDatos.AppSocial.Entities.Models.Post", "InResponseTo")
                        .WithMany("Responses")
                        .HasForeignKey("InResponseToId");

                    b.HasOne("Urbe.BasesDeDatos.AppSocial.Entities.Models.SocialAppUser", "Poster")
                        .WithMany("Posts")
                        .HasForeignKey("PosterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InResponseTo");

                    b.Navigation("Poster");
                });

            modelBuilder.Entity("Urbe.BasesDeDatos.AppSocial.Entities.Models.Post", b =>
                {
                    b.Navigation("Responses");
                });

            modelBuilder.Entity("Urbe.BasesDeDatos.AppSocial.Entities.Models.SocialAppUser", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
