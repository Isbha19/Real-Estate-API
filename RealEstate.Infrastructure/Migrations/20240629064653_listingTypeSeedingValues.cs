using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class listingTypeSeedingValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                    table: "ListingTypes",
                    columns: new[] { "Id", "Name" },
                    values: new object[]
                    {
            1,
            "Buy"                    });

            migrationBuilder.InsertData(
                table: "ListingTypes",
                columns: new[] { "Id", "Name" },
                values: new object[]
                {
            2,
            "Rent"                });

            migrationBuilder.InsertData(
                table: "ListingTypes",
                columns: new[] { "Id", "Name"},
                values: new object[]
                {
            3,
            "Commercial"                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
