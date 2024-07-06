using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "LastUpdatedBy", "LastUpdatedOn", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2795), "Central A/C" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2814), "Balcony" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2815), "Shared Spa" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2817), "Concierge Service" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2817), "View of Water" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2818), "Pets Allowed" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2819), "Private Garden" },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2821), "Private Gym" },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2821), "Built in Wardrobes" },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2822), "Built in Kitchen Appliances" },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2823), "Children's Play Area" },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2824), "Maids Room" },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2825), "Shared Pool" },
                    { 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2826), "Shared Gym" },
                    { 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2827), "Covered Parking" },
                    { 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2828), "View of Landmark" },
                    { 17, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2829), "Study" },
                    { 18, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2830), "Private Pool" },
                    { 19, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2831), "Private Jacuzzi" },
                    { 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2833), "Walk-in Closet" },
                    { 21, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2833), "Maid Service" },
                    { 22, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2834), "Children's Pool" },
                    { 23, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(2835), "Barbecue Area" }
                });

            migrationBuilder.InsertData(
                table: "Facilities",
                columns: new[] { "Id", "LastUpdatedBy", "LastUpdatedOn", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3231), "School" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3234), "Hospital" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3235), "Public Transport" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3236), "Shopping Mall" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3237), "Park" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3238), "Metro" }
                });

            migrationBuilder.InsertData(
                table: "FurnishingTypes",
                columns: new[] { "Id", "LastUpdatedBy", "LastUpdatedOn", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3267), "Furnished" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3268), "Unfurnished" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 6, 8, 26, 51, 19, DateTimeKind.Local).AddTicks(3269), "Partly Furnished" }
                });

            migrationBuilder.InsertData(
                table: "ListingTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Rent" },
                    { 2, "Buy" },
                    { 3, "Commercial" }
                });

            migrationBuilder.InsertData(
                table: "businessActivityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Real Estate Brokerage" },
                    { 2, "Property Management" }
                });

            migrationBuilder.InsertData(
                table: "companyStructures",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Sole Proprietorship" },
                    { 2, "Limited Liability Company" },
                    { 3, "Civil Company" },
                    { 4, "Free Zone Establishment)" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FurnishingTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ListingTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ListingTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ListingTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "businessActivityTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "businessActivityTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "companyStructures",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "companyStructures",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "companyStructures",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "companyStructures",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
