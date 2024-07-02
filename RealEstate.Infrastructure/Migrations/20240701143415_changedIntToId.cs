using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedIntToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
               name: "BusinessActivityTypeId",
               table: "Companies",
               type: "int",
               nullable: false,
               oldClrType: typeof(string),
               oldType: "nvarchar(max)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
