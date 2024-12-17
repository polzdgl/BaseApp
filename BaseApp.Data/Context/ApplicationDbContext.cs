using BaseApp.Data.LogTable.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Parameterless constructor for EF Core tools
        public ApplicationDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Provide a fallback connection string
                optionsBuilder.UseSqlServer("Data Source=localhost,1433;Initial Catalog=OneKleren;User Id=sa;Password=@100BlueReaperWork;Encrypt=False;MultipleActiveResultSets=True");
            }
        }

        //public DbSet<UserAccounts> UserAccounts { get; set; }
        public DbSet<User.Models.User> User { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set the default schema for the DbContext
            modelBuilder.HasDefaultSchema("App");

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

            modelBuilder.Entity<User.Models.User>(entity =>
            {
                entity.ToTable("User", schema: "App");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
