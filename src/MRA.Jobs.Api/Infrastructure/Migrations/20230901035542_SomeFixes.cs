using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SomeFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyQuestion_Vacancies_VacancyId",
                table: "VacancyQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_VacancyResponses_VacancyQuestion_QuestionId",
                table: "VacancyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacancyQuestion",
                table: "VacancyQuestion");

            migrationBuilder.RenameTable(
                name: "VacancyQuestion",
                newName: "VacancyQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_VacancyQuestion_VacancyId",
                table: "VacancyQuestions",
                newName: "IX_VacancyQuestions_VacancyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacancyQuestions",
                table: "VacancyQuestions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyQuestions_Vacancies_VacancyId",
                table: "VacancyQuestions",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyResponses_VacancyQuestions_QuestionId",
                table: "VacancyResponses",
                column: "QuestionId",
                principalTable: "VacancyQuestions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyQuestions_Vacancies_VacancyId",
                table: "VacancyQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_VacancyResponses_VacancyQuestions_QuestionId",
                table: "VacancyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacancyQuestions",
                table: "VacancyQuestions");

            migrationBuilder.RenameTable(
                name: "VacancyQuestions",
                newName: "VacancyQuestion");

            migrationBuilder.RenameIndex(
                name: "IX_VacancyQuestions_VacancyId",
                table: "VacancyQuestion",
                newName: "IX_VacancyQuestion_VacancyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacancyQuestion",
                table: "VacancyQuestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyQuestion_Vacancies_VacancyId",
                table: "VacancyQuestion",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyResponses_VacancyQuestion_QuestionId",
                table: "VacancyResponses",
                column: "QuestionId",
                principalTable: "VacancyQuestion",
                principalColumn: "Id");
        }
    }
}
