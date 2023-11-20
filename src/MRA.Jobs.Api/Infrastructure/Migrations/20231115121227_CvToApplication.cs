using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CvToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CV",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CV",
                table: "Applications");
        }
    }
}
