using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class packages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfListings",
                table: "Plan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Plan",
                keyColumn: "Id",
                keyValue: "price_1PaegrGFthNCZxNO1kdaJ4ct",
                column: "NumberOfListings",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Plan",
                keyColumn: "Id",
                keyValue: "price_1PZ4m0GFthNCZxNOoti2pHeh",
                column: "NumberOfListings",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Plan",
                keyColumn: "Id",
                keyValue: "price_1PZ4mUGFthNCZxNOwjVLIVd6",
                column: "NumberOfListings",
                value: 50);

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 10, 1, 8, 20, 965, DateTimeKind.Local).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 10, 1, 8, 20, 965, DateTimeKind.Local).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 10, 1, 8, 20, 965, DateTimeKind.Local).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 10, 1, 8, 20, 965, DateTimeKind.Local).AddTicks(9888));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfListings",
                table: "Plan");

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 22, 25, 22, 118, DateTimeKind.Local).AddTicks(7873));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 22, 25, 22, 118, DateTimeKind.Local).AddTicks(7887));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 22, 25, 22, 118, DateTimeKind.Local).AddTicks(7888));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 22, 25, 22, 118, DateTimeKind.Local).AddTicks(7889));
        }
    }
}
