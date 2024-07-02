using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogo",
                table: "companies");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
