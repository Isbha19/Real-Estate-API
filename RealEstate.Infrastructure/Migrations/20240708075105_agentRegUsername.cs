using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class agentRegUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_AspNetUsers_userId",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Agents");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Agents",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Agents_userId",
                table: "Agents",
                newName: "IX_Agents_UserId");
  migrationBuilder.AddForeignKey(
                name: "FK_Agents_AspNetUsers_UserId",
                table: "Agents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_AspNetUsers_UserId",
                table: "Agents");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Agents",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Agents_UserId",
                table: "Agents",
                newName: "IX_Agents_userId");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(196));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(218));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 5,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(222));

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 6,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(224));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(408));

            migrationBuilder.UpdateData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 7, 17, 50, 38, 882, DateTimeKind.Local).AddTicks(409));

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_AspNetUsers_userId",
                table: "Agents",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
