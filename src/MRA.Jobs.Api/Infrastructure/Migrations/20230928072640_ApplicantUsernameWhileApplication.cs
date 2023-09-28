using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplicantUsernameWhileApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantUsername",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantUsername",
                table: "Applications");
        }
    }
}
