﻿// <auto-generated />
using System;
using BaseApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BaseApp.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("App")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BaseApp.Data.LogTable.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Application")
                        .HasMaxLength(200)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ClientIp")
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Exception")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasMaxLength(200)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("MachineName")
                        .HasMaxLength(200)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Message")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProcessId")
                        .HasColumnType("int");

                    b.Property<string>("ProcessName")
                        .HasMaxLength(200)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Properties")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestHeader")
                        .HasMaxLength(2000)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int?>("ThreadId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TimeStamp")
                        .HasColumnType("datetime");

                    b.Property<string>("UserName")
                        .HasMaxLength(200)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id")
                        .HasName("PK_Logs");

                    b.HasIndex("Application")
                        .HasDatabaseName("IX_Log_Application");

                    b.HasIndex("CorrelationId")
                        .HasDatabaseName("IX_Log_CorrelationId");

                    b.HasIndex("Level")
                        .HasDatabaseName("IX_Log_Level");

                    b.HasIndex("TimeStamp")
                        .HasDatabaseName("IX_Log_TimeStamp");

                    b.HasIndex("UserName")
                        .HasDatabaseName("IX_Log_UserName");

                    b.HasIndex("TimeStamp", "Level", "CorrelationId")
                        .HasDatabaseName("IX_Log_TimeStamp_Level_CorrelationId");

                    b.ToTable("Log", "App");
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.EdgarCompanyInfo", b =>
                {
                    b.Property<string>("Cik")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(10)");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Cik")
                        .HasName("PK_EdgarCompanyInfo");

                    b.ToTable("EdgarCompanyInfo", "Sec");
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EdgarCompanyInfoId")
                        .IsRequired()
                        .HasColumnType("CHAR(10)");

                    b.HasKey("Id")
                        .HasName("PK_InfoFact");

                    b.HasIndex("EdgarCompanyInfoId")
                        .IsUnique();

                    b.ToTable("InfoFact", "Sec");
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InfoFactId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_InfoFactUsGaap");

                    b.HasIndex("InfoFactId")
                        .IsUnique();

                    b.ToTable("InfoFactUsGaap", "Sec");

                    b.HasAnnotation("Relational:JsonPropertyName", "us-gaap");
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapIncomeLossUnits", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InfoFactUsGaapNetIncomeLossId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_InfoFactUsGaapIncomeLossUnits");

                    b.HasIndex("InfoFactUsGaapNetIncomeLossId")
                        .IsUnique();

                    b.ToTable("InfoFactUsGaapIncomeLossUnits", "Sec");
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapIncomeLossUnitsUsd", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Form")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Frame")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("InfoFactUsGaapIncomeLossUnitsId")
                        .HasColumnType("int");

                    b.Property<decimal>("Val")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id")
                        .HasName("PK_InfoFactUsGaapIncomeLossUnitsUsd");

                    b.HasIndex("InfoFactUsGaapIncomeLossUnitsId");

                    b.ToTable("InfoFactUsGaapIncomeLossUnitsUsd", "Sec");
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapNetIncomeLoss", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InfoFactUsGaapId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_InfoFactUsGaapNetIncomeLoss");

                    b.HasIndex("InfoFactUsGaapId")
                        .IsUnique();

                    b.ToTable("InfoFactUsGaapNetIncomeLoss", "Sec");
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Permissions")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("ApplicationRole", "User");
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Source")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("VerifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("ApplicationRoleClaim ", "User");
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("LastName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

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

                    b.ToTable("ApplicationUser", "User");
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("VerifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ApplicationUserClaim ", "User");
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProviderDetails")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("ApplicationUserLogin ", "User");
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssignedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("DateAssigned")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("ApplicationUserRole ", "User");
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUserToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("ApplicationUserToken  ", "User");
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFact", b =>
                {
                    b.HasOne("BaseApp.Data.SecurityExchange.Models.EdgarCompanyInfo", null)
                        .WithOne("Facts")
                        .HasForeignKey("BaseApp.Data.SecurityExchange.Models.InfoFact", "EdgarCompanyInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaap", b =>
                {
                    b.HasOne("BaseApp.Data.SecurityExchange.Models.InfoFact", null)
                        .WithOne("UsGaap")
                        .HasForeignKey("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaap", "InfoFactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapIncomeLossUnits", b =>
                {
                    b.HasOne("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapNetIncomeLoss", null)
                        .WithOne("Units")
                        .HasForeignKey("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapIncomeLossUnits", "InfoFactUsGaapNetIncomeLossId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapIncomeLossUnitsUsd", b =>
                {
                    b.HasOne("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapIncomeLossUnits", null)
                        .WithMany("Usd")
                        .HasForeignKey("InfoFactUsGaapIncomeLossUnitsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapNetIncomeLoss", b =>
                {
                    b.HasOne("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaap", null)
                        .WithOne("NetIncomeLoss")
                        .HasForeignKey("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapNetIncomeLoss", "InfoFactUsGaapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationRoleClaim", b =>
                {
                    b.HasOne("BaseApp.Data.User.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUserClaim", b =>
                {
                    b.HasOne("BaseApp.Data.User.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUserLogin", b =>
                {
                    b.HasOne("BaseApp.Data.User.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUserRole", b =>
                {
                    b.HasOne("BaseApp.Data.User.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BaseApp.Data.User.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.User.Models.ApplicationUserToken", b =>
                {
                    b.HasOne("BaseApp.Data.User.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.EdgarCompanyInfo", b =>
                {
                    b.Navigation("Facts")
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFact", b =>
                {
                    b.Navigation("UsGaap")
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaap", b =>
                {
                    b.Navigation("NetIncomeLoss")
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapIncomeLossUnits", b =>
                {
                    b.Navigation("Usd");
                });

            modelBuilder.Entity("BaseApp.Data.SecurityExchange.Models.InfoFactUsGaapNetIncomeLoss", b =>
                {
                    b.Navigation("Units")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
