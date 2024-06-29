using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class listingTypeSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "ListingTypes");

            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "ListingTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedBy",
                table: "ListingTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "ListingTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ListingTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 29, 10, 41, 31, 549, DateTimeKind.Local).AddTicks(5660) });

            migrationBuilder.UpdateData(
                table: "ListingTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 29, 10, 41, 31, 549, DateTimeKind.Local).AddTicks(5677) });

            migrationBuilder.UpdateData(
                table: "ListingTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 29, 10, 41, 31, 549, DateTimeKind.Local).AddTicks(5678) });
        }
    }
}
