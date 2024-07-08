using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class datachange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

         
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4501));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4518));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4520));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4521));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4521));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4522));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 7,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4523));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 8,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4524));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 9,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4525));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 10,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4526));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 11,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4526));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 12,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4527));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 13,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4528));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 14,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4529));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 15,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4529));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 16,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4530));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 17,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4531));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 18,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4532));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 19,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4534));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 20,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4535));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 21,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4536));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 22,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4537));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 23,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4537));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4765));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4768));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4769));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4770));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4771));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4771));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4797));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4800));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 10, 36, 28, 150, DateTimeKind.Local).AddTicks(4800));
        }
    }
}
