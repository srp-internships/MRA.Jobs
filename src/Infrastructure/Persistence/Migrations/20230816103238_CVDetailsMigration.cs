using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CVDetailsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationDetails",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniversityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateDegreeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDetails", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceDetails",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JobLocationCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobLocationCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceDetails", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationDetails");

            migrationBuilder.DropTable(
                name: "ExperienceDetails");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
