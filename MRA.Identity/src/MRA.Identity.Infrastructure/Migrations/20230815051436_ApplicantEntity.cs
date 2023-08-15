using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplicantEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantUser",
                table: "UserSkills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantUser",
                table: "UserExperiences",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantUser",
                table: "UserEducations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    User = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.User);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_ApplicantUser",
                table: "UserSkills",
                column: "ApplicantUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserExperiences_ApplicantUser",
                table: "UserExperiences",
                column: "ApplicantUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserEducations_ApplicantUser",
                table: "UserEducations",
                column: "ApplicantUser");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEducations_Applicants_ApplicantUser",
                table: "UserEducations",
                column: "ApplicantUser",
                principalTable: "Applicants",
                principalColumn: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExperiences_Applicants_ApplicantUser",
                table: "UserExperiences",
                column: "ApplicantUser",
                principalTable: "Applicants",
                principalColumn: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Applicants_ApplicantUser",
                table: "UserSkills",
                column: "ApplicantUser",
                principalTable: "Applicants",
                principalColumn: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEducations_Applicants_ApplicantUser",
                table: "UserEducations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExperiences_Applicants_ApplicantUser",
                table: "UserExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Applicants_ApplicantUser",
                table: "UserSkills");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_UserSkills_ApplicantUser",
                table: "UserSkills");

            migrationBuilder.DropIndex(
                name: "IX_UserExperiences_ApplicantUser",
                table: "UserExperiences");

            migrationBuilder.DropIndex(
                name: "IX_UserEducations_ApplicantUser",
                table: "UserEducations");

            migrationBuilder.DropColumn(
                name: "ApplicantUser",
                table: "UserSkills");

            migrationBuilder.DropColumn(
                name: "ApplicantUser",
                table: "UserExperiences");

            migrationBuilder.DropColumn(
                name: "ApplicantUser",
                table: "UserEducations");
        }
    }
}
