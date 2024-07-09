using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class susbcriptionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plan",
                keyColumn: "Id",
                keyValue: "p1");

            migrationBuilder.InsertData(
                table: "Plan",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[] { "price_1PaegrGFthNCZxNO1kdaJ4ct", "Starter", 100 });

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 18, 20, 46, 193, DateTimeKind.Local).AddTicks(3008));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 18, 20, 46, 193, DateTimeKind.Local).AddTicks(3023));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 18, 20, 46, 193, DateTimeKind.Local).AddTicks(3024));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 18, 20, 46, 193, DateTimeKind.Local).AddTicks(3025));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plan",
                keyColumn: "Id",
                keyValue: "price_1PaegrGFthNCZxNO1kdaJ4ct");

            migrationBuilder.InsertData(
                table: "Plan",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[] { "p1", "Free Trial", 0 });

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 14, 48, 45, 643, DateTimeKind.Local).AddTicks(3229));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 14, 48, 45, 643, DateTimeKind.Local).AddTicks(3243));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 14, 48, 45, 643, DateTimeKind.Local).AddTicks(3244));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 14, 48, 45, 643, DateTimeKind.Local).AddTicks(3245));
        }
    }
}
