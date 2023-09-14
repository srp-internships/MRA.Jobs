using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfirmationCodesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfirmationCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmationCodes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmationCodes");
        }
    }
}
