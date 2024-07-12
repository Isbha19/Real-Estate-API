using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editedMessageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRead",
                table: "Messages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RecipientDeleted",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SenderDeleted",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 12, 7, 44, 1, 498, DateTimeKind.Local).AddTicks(2778));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 12, 7, 44, 1, 498, DateTimeKind.Local).AddTicks(2795));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 12, 7, 44, 1, 498, DateTimeKind.Local).AddTicks(2796));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 12, 7, 44, 1, 498, DateTimeKind.Local).AddTicks(2797));

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DateRead",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "RecipientDeleted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderDeleted",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 11, 15, 47, 41, 708, DateTimeKind.Local).AddTicks(2989));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 11, 15, 47, 41, 708, DateTimeKind.Local).AddTicks(3005));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 11, 15, 47, 41, 708, DateTimeKind.Local).AddTicks(3005));

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "LastUpdatedOn",
                value: new DateTime(2024, 7, 11, 15, 47, 41, 708, DateTimeKind.Local).AddTicks(3007));

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
