using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtimeAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoryWithConnectionStringAndServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2024, 7, 20, 14, 16, 29, 473, DateTimeKind.Local).AddTicks(6090));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2024, 7, 20, 14, 16, 29, 473, DateTimeKind.Local).AddTicks(6114));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2024, 7, 20, 14, 16, 29, 473, DateTimeKind.Local).AddTicks(6117));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2024, 7, 20, 14, 14, 12, 425, DateTimeKind.Local).AddTicks(5176));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2024, 7, 20, 14, 14, 12, 425, DateTimeKind.Local).AddTicks(5197));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2024, 7, 20, 14, 14, 12, 425, DateTimeKind.Local).AddTicks(5202));
        }
    }
}
