using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class agentVeriification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCompanyAdminVerified",
                table: "Agents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "licenseNumber",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCompanyAdminVerified",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "licenseNumber",
                table: "Agents");

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6202));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6219));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6220));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6221));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6221));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6222));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6243));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6244));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 11, 51, 5, 140, DateTimeKind.Local).AddTicks(6245));
        }
    }
}
