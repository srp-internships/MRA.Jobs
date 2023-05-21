using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameVacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassEndDate",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "ClassStartDate",
                table: "Vacancies");

            migrationBuilder.RenameColumn(
                name: "Internship_Duration",
                table: "Vacancies",
                newName: "InternshipVacancy_Duration");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentityId",
                table: "DomainUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "DomainUsers");

            migrationBuilder.RenameColumn(
                name: "InternshipVacancy_Duration",
                table: "Vacancies",
                newName: "Internship_Duration");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClassEndDate",
                table: "Vacancies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClassStartDate",
                table: "Vacancies",
                type: "datetime2",
                nullable: true);
        }
    }
}
