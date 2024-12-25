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

        //public DbSet<UserAccounts> UserAccounts { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Log> Logs { get; set; }

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

                //entity.HasIndex(e => e.Email)
                //.HasDatabaseName("XI_User_Email")
                //.IsUnique(true);
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
        }
    }
}
