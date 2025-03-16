using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMarketDataLoaded",
                schema: "Sec",
                table: "PublicCompany",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                schema: "Sec",
                table: "PublicCompany",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMarketDataLoaded",
                schema: "Sec",
                table: "PublicCompany");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                schema: "Sec",
                table: "PublicCompany");
        }
    }
}
