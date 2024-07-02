using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class companyLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_companies_Images_CompanyLogoId",
                table: "companies");

            migrationBuilder.DropIndex(
                name: "IX_companies_CompanyLogoId",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "CompanyLogoId",
                table: "companies");

            migrationBuilder.CreateTable(
                name: "CompanyFile",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyFile_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFile_CompanyId",
                table: "CompanyFile",
                column: "CompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyFile");

            migrationBuilder.AddColumn<int>(
                name: "CompanyLogoId",
                table: "companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_companies_CompanyLogoId",
                table: "companies",
                column: "CompanyLogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_companies_Images_CompanyLogoId",
                table: "companies",
                column: "CompanyLogoId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
