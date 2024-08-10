using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Billy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addcomp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Addresss", "City", "Name", "PhoneNumber", "PostalCode", "State" },
                values: new object[,]
                {
                    { 1, "Apprt 305, NY", "New York", "Tech Sol", "090078601", "32222", "WS" },
                    { 2, "Apprt 305, NY", "New York", "Vivid Books", "090078601", "32222", "WS" },
                    { 3, "Apprt 305, NY", "New York", "Reader Club", "090078601", "32222", "WS" }
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
        }
    }
}
