using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Renamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyResponses_VacancyQuestions_QuestionId",
                table: "VacancyResponses");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "VacancyResponses",
                newName: "VacancyQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_VacancyResponses_QuestionId",
                table: "VacancyResponses",
                newName: "IX_VacancyResponses_VacancyQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyResponses_VacancyQuestions_VacancyQuestionId",
                table: "VacancyResponses",
                column: "VacancyQuestionId",
                principalTable: "VacancyQuestions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyResponses_VacancyQuestions_VacancyQuestionId",
                table: "VacancyResponses");

            migrationBuilder.RenameColumn(
                name: "VacancyQuestionId",
                table: "VacancyResponses",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_VacancyResponses_VacancyQuestionId",
                table: "VacancyResponses",
                newName: "IX_VacancyResponses_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyResponses_VacancyQuestions_QuestionId",
                table: "VacancyResponses",
                column: "QuestionId",
                principalTable: "VacancyQuestions",
                principalColumn: "Id");
        }
    }
}
