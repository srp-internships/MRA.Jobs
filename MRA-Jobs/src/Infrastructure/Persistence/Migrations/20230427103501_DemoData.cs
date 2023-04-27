using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA_Jobs.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vacancy",
                columns: new[] { "Id", "CreatedAt", "Description", "Discriminator", "EndDate", "PublishDate", "RequeredYearOfExperience", "ShortDescription", "Title" },
                values: new object[] { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tersd", "JobVacancy", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "sad", "Senior C# backend developer" });

            migrationBuilder.InsertData(
                table: "Vacancy",
                columns: new[] { "Id", "ClassEndDate", "ClassStartDate", "CreatedAt", "Description", "Discriminator", "EndDate", "PublishDate", "ShortDescription", "Title" },
                values: new object[] { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tersd", "EducationVacancy", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sad", "Training class" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vacancy",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Vacancy",
                keyColumn: "Id",
                keyValue: 5L);
        }
    }
}
