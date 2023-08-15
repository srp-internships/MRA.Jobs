using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Applicant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "ApplicantUser",
                table: "UserSkills",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkills_ApplicantUser",
                table: "UserSkills",
                newName: "IX_UserSkills_ApplicantId");

            migrationBuilder.RenameColumn(
                name: "ApplicantUser",
                table: "UserExperiences",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_UserExperiences_ApplicantUser",
                table: "UserExperiences",
                newName: "IX_UserExperiences_ApplicantId");

            migrationBuilder.RenameColumn(
                name: "ApplicantUser",
                table: "UserEducations",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEducations_ApplicantUser",
                table: "UserEducations",
                newName: "IX_UserEducations_ApplicantId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEducations_AspNetUsers_ApplicantId",
                table: "UserEducations",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExperiences_AspNetUsers_ApplicantId",
                table: "UserExperiences",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_AspNetUsers_ApplicantId",
                table: "UserSkills",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEducations_AspNetUsers_ApplicantId",
                table: "UserEducations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExperiences_AspNetUsers_ApplicantId",
                table: "UserExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_AspNetUsers_ApplicantId",
                table: "UserSkills");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "UserSkills",
                newName: "ApplicantUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkills_ApplicantId",
                table: "UserSkills",
                newName: "IX_UserSkills_ApplicantUser");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "UserExperiences",
                newName: "ApplicantUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserExperiences_ApplicantId",
                table: "UserExperiences",
                newName: "IX_UserExperiences_ApplicantUser");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "UserEducations",
                newName: "ApplicantUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserEducations_ApplicantId",
                table: "UserEducations",
                newName: "IX_UserEducations_ApplicantUser");

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
    }
}
