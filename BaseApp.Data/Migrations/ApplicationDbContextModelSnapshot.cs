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
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BaseApp.Data.Company.Models.CompanyInfo", b =>
                {
                    b.Property<string>("Cik")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(10)")
                        .HasAnnotation("Relational:JsonPropertyName", "cik");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasAnnotation("Relational:JsonPropertyName", "entityName");

                    b.HasKey("Cik")
                        .HasName("PK_CompanyInfo");

                    b.ToTable("CompanyInfo", "Sec");
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyInfoId")
                        .IsRequired()
                        .HasColumnType("CHAR(10)");

                    b.HasKey("Id")
                        .HasName("PK_InfoFact");

                    b.HasIndex("CompanyInfoId")
                        .IsUnique();

                    b.ToTable("InfoFact", "Sec");

                    b.HasAnnotation("Relational:JsonPropertyName", "facts");
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaap", b =>
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

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaapIncomeLossUnits", b =>
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

                    b.HasAnnotation("Relational:JsonPropertyName", "units");
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaapIncomeLossUnitsUsd", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date")
                        .HasAnnotation("Relational:JsonPropertyName", "end");

                    b.Property<DateOnly?>("FiledAt")
                        .HasColumnType("date")
                        .HasAnnotation("Relational:JsonPropertyName", "filed");

                    b.Property<string>("FiscalPeriod")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "fp");

                    b.Property<int?>("FiscalYear")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fy");

                    b.Property<string>("Form")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasAnnotation("Relational:JsonPropertyName", "form");

                    b.Property<string>("Frame")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasAnnotation("Relational:JsonPropertyName", "frame");

                    b.Property<int>("InfoFactUsGaapIncomeLossUnitsId")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("StartDate")
                        .HasColumnType("date")
                        .HasAnnotation("Relational:JsonPropertyName", "start");

                    b.Property<decimal>("Val")
                        .HasColumnType("decimal(18, 2)")
                        .HasAnnotation("Relational:JsonPropertyName", "val");

                    b.HasKey("Id")
                        .HasName("PK_InfoFactUsGaapIncomeLossUnitsUsd");

                    b.HasIndex("InfoFactUsGaapIncomeLossUnitsId");

                    b.ToTable("InfoFactUsGaapIncomeLossUnitsUsd", "Sec");

                    b.HasAnnotation("Relational:JsonPropertyName", "USD");
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaapNetIncomeLoss", b =>
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

                    b.HasAnnotation("Relational:JsonPropertyName", "NetIncomeLoss");
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.MarketDataLoadRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("LoadDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id")
                        .HasName("PK_MarketDataStatus");

                    b.ToTable("MarketDataStatus", "Sec");
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.PublicCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cik")
                        .IsRequired()
                        .HasColumnType("CHAR(10)")
                        .HasAnnotation("Relational:JsonPropertyName", "cik_str");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cusip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Exchange")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsMarketDataLoaded")
                        .HasColumnType("bit");

                    b.Property<string>("Isin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasAnnotation("Relational:JsonPropertyName", "title");

                    b.Property<string>("Sector")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sedar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubIndustry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasAnnotation("Relational:JsonPropertyName", "ticker");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK_PublicCompany");

                    b.ToTable("PublicCompany", "Sec");
                });

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

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFact", b =>
                {
                    b.HasOne("BaseApp.Data.Company.Models.CompanyInfo", null)
                        .WithOne("InfoFact")
                        .HasForeignKey("BaseApp.Data.Company.Models.InfoFact", "CompanyInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaap", b =>
                {
                    b.HasOne("BaseApp.Data.Company.Models.InfoFact", null)
                        .WithOne("InfoFactUsGaap")
                        .HasForeignKey("BaseApp.Data.Company.Models.InfoFactUsGaap", "InfoFactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaapIncomeLossUnits", b =>
                {
                    b.HasOne("BaseApp.Data.Company.Models.InfoFactUsGaapNetIncomeLoss", null)
                        .WithOne("InfoFactUsGaapIncomeLossUnits")
                        .HasForeignKey("BaseApp.Data.Company.Models.InfoFactUsGaapIncomeLossUnits", "InfoFactUsGaapNetIncomeLossId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaapIncomeLossUnitsUsd", b =>
                {
                    b.HasOne("BaseApp.Data.Company.Models.InfoFactUsGaapIncomeLossUnits", null)
                        .WithMany("InfoFactUsGaapIncomeLossUnitsUsd")
                        .HasForeignKey("InfoFactUsGaapIncomeLossUnitsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaapNetIncomeLoss", b =>
                {
                    b.HasOne("BaseApp.Data.Company.Models.InfoFactUsGaap", null)
                        .WithOne("InfoFactUsGaapNetIncomeLoss")
                        .HasForeignKey("BaseApp.Data.Company.Models.InfoFactUsGaapNetIncomeLoss", "InfoFactUsGaapId")
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

            modelBuilder.Entity("BaseApp.Data.Company.Models.CompanyInfo", b =>
                {
                    b.Navigation("InfoFact")
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFact", b =>
                {
                    b.Navigation("InfoFactUsGaap")
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaap", b =>
                {
                    b.Navigation("InfoFactUsGaapNetIncomeLoss")
                        .IsRequired();
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaapIncomeLossUnits", b =>
                {
                    b.Navigation("InfoFactUsGaapIncomeLossUnitsUsd");
                });

            modelBuilder.Entity("BaseApp.Data.Company.Models.InfoFactUsGaapNetIncomeLoss", b =>
                {
                    b.Navigation("InfoFactUsGaapIncomeLossUnits")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
