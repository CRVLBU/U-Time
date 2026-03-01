using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtimeAPI.Migrations
{
    /// <inheritdoc />
    public partial class UtimeWithCreationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Activities",
                newName: "CreationDate");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2024, 10, 15, 18, 38, 16, 261, DateTimeKind.Local).AddTicks(1267));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2024, 10, 15, 18, 38, 16, 261, DateTimeKind.Local).AddTicks(1282));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2024, 10, 15, 18, 38, 16, 261, DateTimeKind.Local).AddTicks(1284));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Activities",
                newName: "CreatedDate");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2024, 8, 6, 15, 42, 38, 245, DateTimeKind.Local).AddTicks(7592));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2024, 8, 6, 15, 42, 38, 245, DateTimeKind.Local).AddTicks(7611));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2024, 8, 6, 15, 42, 38, 245, DateTimeKind.Local).AddTicks(7612));
        }
    }
}
