using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtimeAPI.Migrations
{
    /// <inheritdoc />
    public partial class UtimeWithActivityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2024, 11, 18, 19, 52, 37, 780, DateTimeKind.Local).AddTicks(1724));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2024, 11, 18, 19, 52, 37, 780, DateTimeKind.Local).AddTicks(1755));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2024, 11, 18, 19, 52, 37, 780, DateTimeKind.Local).AddTicks(1761));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Activities");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2024, 10, 31, 12, 15, 8, 889, DateTimeKind.Local).AddTicks(2879));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2024, 10, 31, 12, 15, 8, 889, DateTimeKind.Local).AddTicks(2900));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2024, 10, 31, 12, 15, 8, 889, DateTimeKind.Local).AddTicks(2902));
        }
    }
}
