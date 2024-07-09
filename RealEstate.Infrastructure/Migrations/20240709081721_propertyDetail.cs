using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class propertyDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Agents_UserId",
                table: "Agents");

            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1366));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1381));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1382));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1383));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1384));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1384));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1385));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1386));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1387));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1388));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 9, 12, 17, 20, 210, DateTimeKind.Local).AddTicks(1389));

            migrationBuilder.CreateIndex(
                name: "IX_Agents_UserId",
                table: "Agents",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Agents_UserId",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5397));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5412));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5412));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5413));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5414));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5415));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5416));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5417));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5418));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5419));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 8, 23, 55, 57, 157, DateTimeKind.Local).AddTicks(5420));

            migrationBuilder.CreateIndex(
                name: "IX_Agents_UserId",
                table: "Agents",
                column: "UserId");
        }
    }
}
