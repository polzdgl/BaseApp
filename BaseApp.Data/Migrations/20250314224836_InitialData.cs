using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.EnsureSchema(
                name: "Sec");

            migrationBuilder.EnsureSchema(
                name: "App");

            migrationBuilder.CreateTable(
                name: "ApplicationRole",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Permissions = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInfo",
                schema: "Sec",
                columns: table => new
                {
                    Cik = table.Column<string>(type: "CHAR(10)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfo", x => x.Cik);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestHeader = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MachineName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Application = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ThreadId = table.Column<int>(type: "int", nullable: true),
                    ProcessId = table.Column<int>(type: "int", nullable: true),
                    ProcessName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClientIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketDataStatus",
                schema: "Sec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoadDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketDataStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublicCompany",
                schema: "Sec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Cik = table.Column<string>(type: "CHAR(10)", nullable: false),
                    Sedar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Isin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cusip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubIndustry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exchange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoleClaim ",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleClaim ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleClaim _ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "User",
                        principalTable: "ApplicationRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserClaim ",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserClaim ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserClaim _ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLogin ",
                schema: "User",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProviderDetails = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLogin ", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_ApplicationUserLogin _ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRole ",
                schema: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRole ", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole _ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "User",
                        principalTable: "ApplicationRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole _ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserToken  ",
                schema: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserToken  ", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_ApplicationUserToken  _ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfoFact",
                schema: "Sec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyInfoId = table.Column<string>(type: "CHAR(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoFact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoFact_CompanyInfo_CompanyInfoId",
                        column: x => x.CompanyInfoId,
                        principalSchema: "Sec",
                        principalTable: "CompanyInfo",
                        principalColumn: "Cik",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfoFactUsGaap",
                schema: "Sec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoFactId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoFactUsGaap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoFactUsGaap_InfoFact_InfoFactId",
                        column: x => x.InfoFactId,
                        principalSchema: "Sec",
                        principalTable: "InfoFact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfoFactUsGaapNetIncomeLoss",
                schema: "Sec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoFactUsGaapId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoFactUsGaapNetIncomeLoss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoFactUsGaapNetIncomeLoss_InfoFactUsGaap_InfoFactUsGaapId",
                        column: x => x.InfoFactUsGaapId,
                        principalSchema: "Sec",
                        principalTable: "InfoFactUsGaap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfoFactUsGaapIncomeLossUnits",
                schema: "Sec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoFactUsGaapNetIncomeLossId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoFactUsGaapIncomeLossUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoFactUsGaapIncomeLossUnits_InfoFactUsGaapNetIncomeLoss_InfoFactUsGaapNetIncomeLossId",
                        column: x => x.InfoFactUsGaapNetIncomeLossId,
                        principalSchema: "Sec",
                        principalTable: "InfoFactUsGaapNetIncomeLoss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfoFactUsGaapIncomeLossUnitsUsd",
                schema: "Sec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InfoFactUsGaapIncomeLossUnitsId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Val = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    FiscalPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiledAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Form = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Frame = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoFactUsGaapIncomeLossUnitsUsd", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoFactUsGaapIncomeLossUnitsUsd_InfoFactUsGaapIncomeLossUnits_InfoFactUsGaapIncomeLossUnitsId",
                        column: x => x.InfoFactUsGaapIncomeLossUnitsId,
                        principalSchema: "Sec",
                        principalTable: "InfoFactUsGaapIncomeLossUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "User",
                table: "ApplicationRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleClaim _RoleId",
                schema: "User",
                table: "ApplicationRoleClaim ",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "User",
                table: "ApplicationUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "User",
                table: "ApplicationUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserClaim _UserId",
                schema: "User",
                table: "ApplicationUserClaim ",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLogin _UserId",
                schema: "User",
                table: "ApplicationUserLogin ",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRole _RoleId",
                schema: "User",
                table: "ApplicationUserRole ",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoFact_CompanyInfoId",
                schema: "Sec",
                table: "InfoFact",
                column: "CompanyInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InfoFactUsGaap_InfoFactId",
                schema: "Sec",
                table: "InfoFactUsGaap",
                column: "InfoFactId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InfoFactUsGaapIncomeLossUnits_InfoFactUsGaapNetIncomeLossId",
                schema: "Sec",
                table: "InfoFactUsGaapIncomeLossUnits",
                column: "InfoFactUsGaapNetIncomeLossId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InfoFactUsGaapIncomeLossUnitsUsd_InfoFactUsGaapIncomeLossUnitsId",
                schema: "Sec",
                table: "InfoFactUsGaapIncomeLossUnitsUsd",
                column: "InfoFactUsGaapIncomeLossUnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoFactUsGaapNetIncomeLoss_InfoFactUsGaapId",
                schema: "Sec",
                table: "InfoFactUsGaapNetIncomeLoss",
                column: "InfoFactUsGaapId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Log_Application",
                schema: "App",
                table: "Log",
                column: "Application");

            migrationBuilder.CreateIndex(
                name: "IX_Log_CorrelationId",
                schema: "App",
                table: "Log",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_Level",
                schema: "App",
                table: "Log",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Log_TimeStamp",
                schema: "App",
                table: "Log",
                column: "TimeStamp");

            migrationBuilder.CreateIndex(
                name: "IX_Log_TimeStamp_Level_CorrelationId",
                schema: "App",
                table: "Log",
                columns: new[] { "TimeStamp", "Level", "CorrelationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Log_UserName",
                schema: "App",
                table: "Log",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRoleClaim ",
                schema: "User");

            migrationBuilder.DropTable(
                name: "ApplicationUserClaim ",
                schema: "User");

            migrationBuilder.DropTable(
                name: "ApplicationUserLogin ",
                schema: "User");

            migrationBuilder.DropTable(
                name: "ApplicationUserRole ",
                schema: "User");

            migrationBuilder.DropTable(
                name: "ApplicationUserToken  ",
                schema: "User");

            migrationBuilder.DropTable(
                name: "InfoFactUsGaapIncomeLossUnitsUsd",
                schema: "Sec");

            migrationBuilder.DropTable(
                name: "Log",
                schema: "App");

            migrationBuilder.DropTable(
                name: "MarketDataStatus",
                schema: "Sec");

            migrationBuilder.DropTable(
                name: "PublicCompany",
                schema: "Sec");

            migrationBuilder.DropTable(
                name: "ApplicationRole",
                schema: "User");

            migrationBuilder.DropTable(
                name: "ApplicationUser",
                schema: "User");

            migrationBuilder.DropTable(
                name: "InfoFactUsGaapIncomeLossUnits",
                schema: "Sec");

            migrationBuilder.DropTable(
                name: "InfoFactUsGaapNetIncomeLoss",
                schema: "Sec");

            migrationBuilder.DropTable(
                name: "InfoFactUsGaap",
                schema: "Sec");

            migrationBuilder.DropTable(
                name: "InfoFact",
                schema: "Sec");

            migrationBuilder.DropTable(
                name: "CompanyInfo",
                schema: "Sec");
        }
    }
}
