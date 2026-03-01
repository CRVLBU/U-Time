using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtimeAPI.Migrations
{
    /// <inheritdoc />
    public partial class UtimeWithUrgImp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Imp",
                table: "Activities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Urgency",
                table: "Activities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2024, 12, 26, 14, 15, 56, 986, DateTimeKind.Local).AddTicks(8823));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2024, 12, 26, 14, 15, 56, 986, DateTimeKind.Local).AddTicks(8837));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2024, 12, 26, 14, 15, 56, 986, DateTimeKind.Local).AddTicks(8839));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imp",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Urgency",
                table: "Activities");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2024, 12, 24, 13, 32, 52, 348, DateTimeKind.Local).AddTicks(3284));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2024, 12, 24, 13, 32, 52, 348, DateTimeKind.Local).AddTicks(3299));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2024, 12, 24, 13, 32, 52, 348, DateTimeKind.Local).AddTicks(3301));
        }
    }
}
