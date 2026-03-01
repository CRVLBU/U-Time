using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UtimeAPI.Migrations
{
    /// <inheritdoc />
    public partial class UTimeWithCatUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Categories");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Amount", "CategoryName", "CreatedTime" },
                values: new object[,]
                {
                    { 1, 1, "Sport", new DateTime(2025, 1, 3, 14, 5, 21, 211, DateTimeKind.Local).AddTicks(91) },
                    { 2, 2, "Family", new DateTime(2025, 1, 3, 14, 5, 21, 211, DateTimeKind.Local).AddTicks(105) },
                    { 3, 3, "University", new DateTime(2025, 1, 3, 14, 5, 21, 211, DateTimeKind.Local).AddTicks(107) }
                });
        }
    }
}
