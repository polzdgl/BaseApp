using BaseApp.Data.Company.Models;
using BaseApp.Data.LogTable.Models;
using BaseApp.Data.User.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<
         ApplicationUser,
         ApplicationRole,
         string,
         ApplicationUserClaim,
         ApplicationUserRole,
         ApplicationUserLogin,
         ApplicationRoleClaim,
         ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Parameterless constructor for EF Core tools
        public ApplicationDbContext() { }

        // Log table
        public DbSet<Log> Logs { get; set; }

        // User tables
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }

        // Security Exchange tables
        public DbSet<EdgarCompanyInfo> EdgarCompanyInfo { get; set; }
        public DbSet<InfoFact> InfoFact { get; set; }
        public DbSet<InfoFactUsGaap> InfoFactUsGaap { get; set; }
        public DbSet<InfoFactUsGaapNetIncomeLoss> InfoFactUsGaapNetIncomeLoss { get; set; }
        public DbSet<InfoFactUsGaapIncomeLossUnits> InfoFactUsGaapIncomeLossUnits { get; set; }
        public DbSet<InfoFactUsGaapIncomeLossUnitsUsd> InfoFactUsGaapIncomeLossUnitsUsd { get; set; }
        public DbSet<MarketDataLoadRecord> MarketDataLoadRecord { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set the default schema for the DbContext
            modelBuilder.HasDefaultSchema("App");

            #region Logs related tables
            // Set the Log table 
            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log", schema: "App");

                // Primary Key
                entity.HasKey(e => e.Id)
                      .HasName("PK_Logs");

                // Columns
                entity.Property(e => e.Id)
                      .IsRequired()
                      .UseIdentityColumn();

                entity.Property(e => e.TimeStamp)
                      .HasColumnType("datetime");

                entity.Property(e => e.Level)
                      .HasMaxLength(200)
                      .IsUnicode(true);

                entity.Property(e => e.Message)
                      .HasColumnType("nvarchar(max)")
                      .IsUnicode(true);

                entity.Property(e => e.Exception)
                      .HasColumnType("nvarchar(max)")
                      .IsUnicode(true);

                entity.Property(e => e.Properties)
                      .HasColumnType("nvarchar(max)")
                      .IsUnicode(true);

                entity.Property(e => e.RequestHeader)
                      .HasMaxLength(2000)
                      .IsUnicode(true);

                entity.Property(e => e.CorrelationId)
                      .HasColumnType("uniqueidentifier");

                entity.Property(e => e.MachineName)
                      .HasMaxLength(200)
                      .IsUnicode(true);

                entity.Property(e => e.Application)
                      .HasMaxLength(200)
                      .IsUnicode(true);

                entity.Property(e => e.ThreadId);

                entity.Property(e => e.ProcessId);

                entity.Property(e => e.ProcessName)
                      .HasMaxLength(200)
                      .IsUnicode(true);

                entity.Property(e => e.ClientIp)
                      .HasMaxLength(50)
                      .IsUnicode(true);

                entity.Property(e => e.UserName)
                      .HasMaxLength(200)
                      .IsUnicode(true);

                // Indexes
                entity.HasIndex(e => e.TimeStamp)
                      .HasDatabaseName("IX_Log_TimeStamp");

                entity.HasIndex(e => e.Level)
                      .HasDatabaseName("IX_Log_Level");

                entity.HasIndex(e => e.CorrelationId)
                      .HasDatabaseName("IX_Log_CorrelationId");

                entity.HasIndex(e => e.Application)
                      .HasDatabaseName("IX_Log_Application");

                entity.HasIndex(e => e.UserName)
                      .HasDatabaseName("IX_Log_UserName");

                // Optional composite index for combined queries
                entity.HasIndex(e => new { e.TimeStamp, e.Level, e.CorrelationId })
                      .HasDatabaseName("IX_Log_TimeStamp_Level_CorrelationId");
            });
            #endregion

            #region User Identity tables
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("ApplicationUser", schema: "User");

                entity.Property(e => e.FirstName).HasMaxLength(256);

                entity.Property(e => e.LastName).HasMaxLength(256);

                entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("ApplicationRole", schema: "User");

                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.Permissions).HasMaxLength(1024);
            });

            modelBuilder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable("ApplicationUserRole ", schema: "User");

                entity.Property(e => e.AssignedBy).HasMaxLength(256);
            });

            modelBuilder.Entity<ApplicationUserClaim>(entity =>
            {
                entity.ToTable("ApplicationUserClaim ", schema: "User");

                entity.Property(e => e.Source).HasMaxLength(256);
            });

            modelBuilder.Entity<ApplicationRoleClaim>(entity =>
            {
                entity.ToTable("ApplicationRoleClaim ", schema: "User");

                entity.Property(e => e.Source).HasMaxLength(256);
            });

            modelBuilder.Entity<ApplicationUserLogin>(entity =>
            {
                entity.ToTable("ApplicationUserLogin ", schema: "User");

                entity.Property(e => e.LoginProviderDetails).HasMaxLength(256);
            });

            modelBuilder.Entity<ApplicationUserToken>(entity =>
            {
                entity.ToTable("ApplicationUserToken  ", schema: "User");
            });
            #endregion

            #region SecurityExchange - EdgarCompanyInfo Related Tables
            // EdgarCompanyInfo
            modelBuilder.Entity<EdgarCompanyInfo>(entity =>
            {
                entity.ToTable("EdgarCompanyInfo", schema: "Sec");

                // Primary Key
                entity.HasKey(e => e.Cik).HasName("PK_EdgarCompanyInfo");

                // Properties
                entity.Property(e => e.Cik)
                      .IsRequired()
                      .HasColumnType("CHAR(10)"); // CIK is fixed length (10 digits)

                entity.Property(e => e.EntityName)
                      .IsRequired()
                      .HasMaxLength(256);

                // Relationship with InfoFacts
                entity.HasOne(e => e.InfoFact)
                      .WithOne()
                      .HasForeignKey<InfoFact>(f => f.EdgarCompanyInfoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // InfoFact
            modelBuilder.Entity<InfoFact>(entity =>
            {
                entity.ToTable("InfoFact", schema: "Sec");

                // Primary Key
                entity.HasKey(f => f.Id).HasName("PK_InfoFact");

                // Relationships
                entity.HasOne(f => f.InfoFactUsGaap)
                      .WithOne()
                      .HasForeignKey<InfoFactUsGaap>(g => g.InfoFactId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // InfoFactUsGaap
            modelBuilder.Entity<InfoFactUsGaap>(entity =>
            {
                entity.ToTable("InfoFactUsGaap", schema: "Sec");

                // Primary Key
                entity.HasKey(g => g.Id).HasName("PK_InfoFactUsGaap");

                // Relationship with NetIncomeLoss
                entity.HasOne(g => g.InfoFactUsGaapNetIncomeLoss)
                      .WithOne()
                      .HasForeignKey<InfoFactUsGaapNetIncomeLoss>(n => n.InfoFactUsGaapId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // InfoFactUsGaapNetIncomeLoss
            modelBuilder.Entity<InfoFactUsGaapNetIncomeLoss>(entity =>
            {
                entity.ToTable("InfoFactUsGaapNetIncomeLoss", schema: "Sec");

                // Primary Key
                entity.HasKey(n => n.Id).HasName("PK_InfoFactUsGaapNetIncomeLoss");

                // Relationships
                entity.HasOne(n => n.InfoFactUsGaapIncomeLossUnits)
                      .WithOne()
                      .HasForeignKey<InfoFactUsGaapIncomeLossUnits>(u => u.InfoFactUsGaapNetIncomeLossId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            // InfoFactUsGaapIncomeLossUnits
            modelBuilder.Entity<InfoFactUsGaapIncomeLossUnits>(entity =>
            {
                entity.ToTable("InfoFactUsGaapIncomeLossUnits", schema: "Sec");

                // Primary Key
                entity.HasKey(u => u.Id).HasName("PK_InfoFactUsGaapIncomeLossUnits");

                // Relationship with InfoFactUsGaapNetIncomeLoss
                entity.HasMany(u => u.InfoFactUsGaapIncomeLossUnitsUsd)
                      .WithOne()
                      .HasForeignKey(usd => usd.InfoFactUsGaapIncomeLossUnitsId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // InfoFactUsGaapIncomeLossUnitsUsd
            modelBuilder.Entity<InfoFactUsGaapIncomeLossUnitsUsd>(entity =>
            {
                entity.ToTable("InfoFactUsGaapIncomeLossUnitsUsd", schema: "Sec");

                // Primary Key
                entity.HasKey(usd => usd.Id).HasName("PK_InfoFactUsGaapIncomeLossUnitsUsd");

                // Properties
                entity.Property(usd => usd.Form)
                      .HasMaxLength(10) // E.g., "10-Q", "10-K"
                      .IsRequired();

                entity.Property(usd => usd.Frame)
                      .HasMaxLength(10) // E.g., "CY2021"
                      .IsRequired();

                entity.Property(usd => usd.Val)
                      .HasColumnType("decimal(18, 2)")
                      .IsRequired();
            });

            // MarketDataStatus
            modelBuilder.Entity<MarketDataLoadRecord>(entity =>
            {
                entity.ToTable("MarketDataStatus", schema: "Sec");
                // Primary Key
                entity.HasKey(e => e.Id).HasName("PK_MarketDataStatus");
                // Properties
                entity.Property(e => e.LoadDate)
                      .HasColumnType("datetime")
                      .IsRequired();
            });
            #endregion
        }
    }
}
