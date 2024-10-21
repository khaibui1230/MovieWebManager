using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movie.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabaseOfCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress" },
                values: new object[,]
                {
                    { 1, "San Francisco", "TechWorld", "123-456-7890", "94016", "CA", "123 Silicon Valley" },
                    { 2, "New York", "Book Haven", "987-654-3210", "10001", "NY", "456 Book Street" },
                    { 3, "Seattle", "GadgetWorks", "555-123-4567", "98101", "WA", "789 Tech Drive" },
                    { 4, "Portland", "Green Solutions", "444-987-1234", "97201", "OR", "321 Eco Lane" },
                    { 5, "Detroit", "AutoParts Co.", "888-456-7890", "48201", "MI", "101 Motorway Blvd" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
