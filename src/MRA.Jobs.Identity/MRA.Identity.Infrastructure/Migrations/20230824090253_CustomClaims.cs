using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CustomClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "AspNetUserClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "AspNetUserClaims");
        }
    }
}
