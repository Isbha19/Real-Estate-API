using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedPropertyTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PropertyTypes",
                columns: new[] { "Id", "LastUpdatedBy", "LastUpdatedOn", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5397), "Apartment" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5412), "Villa" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5412), "Townhouse" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5413), "Penthouse" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5414), "Compound" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5415), "Duplex" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5416), "Full Floor" },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5417), "Half floor" },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5418), "Whole building" },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5419), "Land" },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5420), "Hotel Apartment" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
