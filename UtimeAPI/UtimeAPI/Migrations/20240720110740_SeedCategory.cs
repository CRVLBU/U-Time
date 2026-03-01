using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UtimeAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Amount", "CategoryName", "CreatedTime" },
                values: new object[,]
                {
                    { 1, 1, "Sport", new DateTime(2024, 7, 20, 14, 7, 38, 9, DateTimeKind.Local).AddTicks(5952) },
                    { 2, 2, "Family", new DateTime(2024, 7, 20, 14, 7, 38, 9, DateTimeKind.Local).AddTicks(5974) },
                    { 3, 3, "University", new DateTime(2024, 7, 20, 14, 7, 38, 9, DateTimeKind.Local).AddTicks(5980) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
