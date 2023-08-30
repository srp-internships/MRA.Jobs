using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "AspNetUserRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "AspNetUserClaims",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "AspNetUserClaims");
        }
    }
}
