﻿// <auto-generated />
using System;
using DevTrack.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221227131324_AddEmailQueue")]
    partial class AddEmailQueue
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ActiveWindows", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MainWindowTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("ActiveWindows");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("bit");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.EmailQueue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SendStatus")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EmailQueue");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.Invitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.KeyboardActivities", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("KeyCounts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalHits")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId")
                        .IsUnique();

                    b.ToTable("KeyboardActivities");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.MouseActivities", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TotalHits")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId")
                        .IsUnique();

                    b.ToTable("MouseActivities");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AllowActiveWindow")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowKeyboardHit")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowManualTimeEntry")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowMouseClick")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowRunningProgram")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowScreenshot")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowWebcam")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<int>("TimeInterval")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ProjectUser", b =>
                {
                    b.Property<Guid>("ApplicationUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("ApplicationUserId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectUsers");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.RunningProgram", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MainWindowTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProcessName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("RunningPrograms");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ScreenCapture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId")
                        .IsUnique();

                    b.ToTable("ScreenCaptures");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.WebcamCapture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId")
                        .IsUnique();

                    b.ToTable("WebcamCaptures");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ActiveWindows", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.Activity", "Activity")
                        .WithMany("ActiveWindows")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.Activity", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevTrack.Infrastructure.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationRoleClaim", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUserClaim", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUserLogin", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUserRole", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevTrack.Infrastructure.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUserToken", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.Invitation", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.KeyboardActivities", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.Activity", "Activity")
                        .WithOne("KeyboardActivity")
                        .HasForeignKey("DevTrack.Infrastructure.Entities.KeyboardActivities", "ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.MouseActivities", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.Activity", "Activity")
                        .WithOne("MouseActivity")
                        .HasForeignKey("DevTrack.Infrastructure.Entities.MouseActivities", "ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ProjectUser", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany("UserProjects")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevTrack.Infrastructure.Entities.Project", "Project")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.RunningProgram", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.Activity", "Activity")
                        .WithMany("RunningPrograms")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ScreenCapture", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.Activity", "Activity")
                        .WithOne("ScreenCapture")
                        .HasForeignKey("DevTrack.Infrastructure.Entities.ScreenCapture", "ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.WebcamCapture", b =>
                {
                    b.HasOne("DevTrack.Infrastructure.Entities.Activity", "Activity")
                        .WithOne("WebcamCapture")
                        .HasForeignKey("DevTrack.Infrastructure.Entities.WebcamCapture", "ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.Activity", b =>
                {
                    b.Navigation("ActiveWindows");

                    b.Navigation("KeyboardActivity");

                    b.Navigation("MouseActivity");

                    b.Navigation("RunningPrograms");

                    b.Navigation("ScreenCapture");

                    b.Navigation("WebcamCapture");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.ApplicationUser", b =>
                {
                    b.Navigation("UserProjects");
                });

            modelBuilder.Entity("DevTrack.Infrastructure.Entities.Project", b =>
                {
                    b.Navigation("ProjectUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
