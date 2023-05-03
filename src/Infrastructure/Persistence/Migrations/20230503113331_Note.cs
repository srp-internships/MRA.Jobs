using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Note : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationNote",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Date",
                value: new DateTime(2023, 5, 3, 11, 33, 31, 793, DateTimeKind.Utc).AddTicks(3580));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationNote",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Date",
                value: new DateTime(2023, 5, 2, 11, 6, 53, 299, DateTimeKind.Utc).AddTicks(8887));
        }
    }
}
