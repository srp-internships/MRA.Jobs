using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Applications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_Applicant_ApplicantId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_Application_Vacancy_VacancyId",
                table: "Application");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Application",
                table: "Application");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applicant",
                table: "Applicant");

            migrationBuilder.RenameTable(
                name: "Application",
                newName: "Applications");

            migrationBuilder.RenameTable(
                name: "Applicant",
                newName: "Applicants");

            migrationBuilder.RenameIndex(
                name: "IX_Application_VacancyId",
                table: "Applications",
                newName: "IX_Applications_VacancyId");

            migrationBuilder.RenameIndex(
                name: "IX_Application_ApplicantId",
                table: "Applications",
                newName: "IX_Applications_ApplicantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applicants",
                table: "Applicants",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "ApplicationNote",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Date",
                value: new DateTime(2023, 5, 3, 11, 41, 7, 882, DateTimeKind.Utc).AddTicks(2815));

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Applicants_ApplicantId",
                table: "Applications",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Vacancy_VacancyId",
                table: "Applications",
                column: "VacancyId",
                principalTable: "Vacancy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Applicants_ApplicantId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Vacancy_VacancyId",
                table: "Applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applicants",
                table: "Applicants");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "Application");

            migrationBuilder.RenameTable(
                name: "Applicants",
                newName: "Applicant");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_VacancyId",
                table: "Application",
                newName: "IX_Application_VacancyId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_ApplicantId",
                table: "Application",
                newName: "IX_Application_ApplicantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Application",
                table: "Application",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applicant",
                table: "Applicant",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "ApplicationNote",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Date",
                value: new DateTime(2023, 5, 2, 11, 6, 53, 299, DateTimeKind.Utc).AddTicks(8887));

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Applicant_ApplicantId",
                table: "Application",
                column: "ApplicantId",
                principalTable: "Applicant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Application_Vacancy_VacancyId",
                table: "Application",
                column: "VacancyId",
                principalTable: "Vacancy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
