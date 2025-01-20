using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoFact_CompanyInfo_EdgarCompanyInfoId",
                schema: "Sec",
                table: "InfoFact");

            migrationBuilder.RenameColumn(
                name: "EdgarCompanyInfoId",
                schema: "Sec",
                table: "InfoFact",
                newName: "CompanyInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoFact_EdgarCompanyInfoId",
                schema: "Sec",
                table: "InfoFact",
                newName: "IX_InfoFact_CompanyInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoFact_CompanyInfo_CompanyInfoId",
                schema: "Sec",
                table: "InfoFact",
                column: "CompanyInfoId",
                principalSchema: "Sec",
                principalTable: "CompanyInfo",
                principalColumn: "Cik",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoFact_CompanyInfo_CompanyInfoId",
                schema: "Sec",
                table: "InfoFact");

            migrationBuilder.RenameColumn(
                name: "CompanyInfoId",
                schema: "Sec",
                table: "InfoFact",
                newName: "EdgarCompanyInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoFact_CompanyInfoId",
                schema: "Sec",
                table: "InfoFact",
                newName: "IX_InfoFact_EdgarCompanyInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoFact_CompanyInfo_EdgarCompanyInfoId",
                schema: "Sec",
                table: "InfoFact",
                column: "EdgarCompanyInfoId",
                principalSchema: "Sec",
                principalTable: "CompanyInfo",
                principalColumn: "Cik",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
