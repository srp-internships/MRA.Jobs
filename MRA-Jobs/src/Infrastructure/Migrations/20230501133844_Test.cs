using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA_Jobs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Date",
                value: new DateTime(2023, 5, 1, 13, 38, 44, 829, DateTimeKind.Utc).AddTicks(3305));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "BirthDay",
                value: new DateTime(2023, 5, 1, 13, 38, 44, 829, DateTimeKind.Utc).AddTicks(764));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Date",
                value: new DateTime(2023, 5, 1, 11, 41, 11, 963, DateTimeKind.Utc).AddTicks(8259));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "BirthDay",
                value: new DateTime(2023, 5, 1, 11, 41, 11, 963, DateTimeKind.Utc).AddTicks(6880));
        }
    }
}
