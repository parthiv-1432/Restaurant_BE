using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29d06340-2cd4-44de-95c6-56dc46105f0e", "a9b34aab-bd06-401a-b9e3-9ef18d32cecd", "Steward", "STEWARD" },
                    { "41cdbecb-8533-49db-a121-e10ca0539cfb", "3616bd65-7ace-4206-9b55-bc3d8e739676", "Administrator", "ADMINISTRATOR" },
                    { "989aa17b-0ae1-4d94-95c0-43cf85eb276c", "3c6c09a8-a2e0-4d26-ac67-a8540b287777", "Biller", "BILLER" },
                    { "a11ecff7-8662-4ff0-90aa-cdb9a91251ac", "65842e9b-11f6-4f15-a61c-90caf9bf002d", "Chef", "CHEF" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29d06340-2cd4-44de-95c6-56dc46105f0e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41cdbecb-8533-49db-a121-e10ca0539cfb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "989aa17b-0ae1-4d94-95c0-43cf85eb276c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a11ecff7-8662-4ff0-90aa-cdb9a91251ac");
        }
    }
}
