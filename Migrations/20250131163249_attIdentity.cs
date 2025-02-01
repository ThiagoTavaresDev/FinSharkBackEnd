using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinSharkProjeto.Migrations
{
    /// <inheritdoc />
    public partial class attIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "220e5806-9387-486a-a3e1-7a064db1ad39", null, "User", "USER" },
                    { "884f7a58-9cd4-4f4e-8ca9-cca9a43c7922", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "220e5806-9387-486a-a3e1-7a064db1ad39");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "884f7a58-9cd4-4f4e-8ca9-cca9a43c7922");
        }
    }
}
