using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSomeFiledsToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Revenue",
                table: "Properties",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "SoldOn",
                table: "Properties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SoldToUserId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSold",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_SoldToUserId",
                table: "Properties",
                column: "SoldToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_SoldToUserId",
                table: "Properties",
                column: "SoldToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_SoldToUserId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_SoldToUserId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SoldOn",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SoldToUserId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "isSold",
                table: "Properties");
        }
    }
}