using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VacancyTagConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyTags_Vacancies_VacancyId",
                table: "VacancyTags");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyTags_Vacancies_VacancyId",
                table: "VacancyTags",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyTags_Vacancies_VacancyId",
                table: "VacancyTags");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyTags_Vacancies_VacancyId",
                table: "VacancyTags",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
