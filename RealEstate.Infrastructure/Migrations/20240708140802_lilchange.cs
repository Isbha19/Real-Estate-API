using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class lilchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "FurnishingTypes");

            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "FurnishingTypes");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "Facilities");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "companies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Agents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Agents");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedBy",
                table: "FurnishingTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "FurnishingTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedBy",
                table: "Facilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "Facilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5400) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5418) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5419) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5420) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5421) });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5422) });

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5452) });

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5453) });

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LastUpdatedBy", "LastUpdatedOn" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 8, 18, 5, 24, 998, DateTimeKind.Local).AddTicks(5454) });
        }
    }
}
