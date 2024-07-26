using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedpaymentLinkField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentLink",
                table: "Plan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Plan",
                keyColumn: "Id",
                keyValue: "price_1PaegrGFthNCZxNO1kdaJ4ct",
                column: "PaymentLink",
                value: null);

            migrationBuilder.UpdateData(
                table: "Plan",
                keyColumn: "Id",
                keyValue: "price_1PZ4m0GFthNCZxNOoti2pHeh",
                column: "PaymentLink",
                value: null);

            migrationBuilder.UpdateData(
                table: "Plan",
                keyColumn: "Id",
                keyValue: "price_1PZ4mUGFthNCZxNOwjVLIVd6",
                column: "PaymentLink",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentLink",
                table: "Plan");
        }
    }
}
