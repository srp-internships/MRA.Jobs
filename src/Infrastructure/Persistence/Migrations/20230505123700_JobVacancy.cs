using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRA.Jobs.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class JobVacancy : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Application_Applicant_ApplicantId",
            table: "Application");

        migrationBuilder.DropTable(
            name: "ApplicationNote");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Applicant",
            table: "Applicant");

        migrationBuilder.RenameTable(
            name: "Applicant",
            newName: "User");

        migrationBuilder.RenameColumn(
            name: "Patronymic",
            table: "User",
            newName: "SocialMediaHandles");

        migrationBuilder.RenameColumn(
            name: "BirthDay",
            table: "User",
            newName: "UpdatedAt");

        migrationBuilder.AlterColumn<long>(
            name: "CategoryId",
            table: "Vacancy",
            type: "bigint",
            nullable: false,
            defaultValue: 0L,
            oldClrType: typeof(long),
            oldType: "bigint",
            oldNullable: true);

        migrationBuilder.AddColumn<long>(
            name: "CreatedBy",
            table: "Vacancy",
            type: "bigint",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "LastModifiedAt",
            table: "Vacancy",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<long>(
            name: "LastModifiedBy",
            table: "Vacancy",
            type: "bigint",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "TestResult",
            table: "Application",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Avatar",
            table: "User",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "User",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "DateOfBrith",
            table: "User",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "Discriminator",
            table: "User",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddPrimaryKey(
            name: "PK_User",
            table: "User",
            column: "Id");

        migrationBuilder.CreateTable(
            name: "Tags",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tags", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "TimelineEvent",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CreateBy = table.Column<long>(type: "bigint", nullable: true),
                Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                EventType = table.Column<int>(type: "int", nullable: false),
                Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ApplicationId = table.Column<long>(type: "bigint", nullable: true),
                UserId = table.Column<long>(type: "bigint", nullable: true),
                VacancyId = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TimelineEvent", x => x.Id);
                table.ForeignKey(
                    name: "FK_TimelineEvent_Application_ApplicationId",
                    column: x => x.ApplicationId,
                    principalTable: "Application",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_TimelineEvent_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_TimelineEvent_Vacancy_VacancyId",
                    column: x => x.VacancyId,
                    principalTable: "Vacancy",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserTags",
            columns: table => new
            {
                TagId = table.Column<long>(type: "bigint", nullable: false),
                UserId = table.Column<long>(type: "bigint", nullable: false),
                Id = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserTags", x => new { x.UserId, x.TagId });
                table.ForeignKey(
                    name: "FK_UserTags_Tags_TagId",
                    column: x => x.TagId,
                    principalTable: "Tags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserTags_User_UserId",
                    column: x => x.UserId,
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "VacancyTags",
            columns: table => new
            {
                TagId = table.Column<long>(type: "bigint", nullable: false),
                VacancyId = table.Column<long>(type: "bigint", nullable: false),
                Id = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VacancyTags", x => new { x.VacancyId, x.TagId });
                table.ForeignKey(
                    name: "FK_VacancyTags_Tags_TagId",
                    column: x => x.TagId,
                    principalTable: "Tags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_VacancyTags_Vacancy_VacancyId",
                    column: x => x.VacancyId,
                    principalTable: "Vacancy",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.UpdateData(
            table: "Vacancy",
            keyColumn: "Id",
            keyValue: 3L,
            columns: new[] { "CategoryId", "CreatedBy", "LastModifiedAt", "LastModifiedBy" },
            values: new object[] { 0L, null, null, null });

        migrationBuilder.UpdateData(
            table: "Vacancy",
            keyColumn: "Id",
            keyValue: 5L,
            columns: new[] { "CategoryId", "CreatedBy", "LastModifiedAt", "LastModifiedBy" },
            values: new object[] { 0L, null, null, null });

        migrationBuilder.CreateIndex(
            name: "IX_TimelineEvent_ApplicationId",
            table: "TimelineEvent",
            column: "ApplicationId");

        migrationBuilder.CreateIndex(
            name: "IX_TimelineEvent_UserId",
            table: "TimelineEvent",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_TimelineEvent_VacancyId",
            table: "TimelineEvent",
            column: "VacancyId");

        migrationBuilder.CreateIndex(
            name: "IX_UserTags_TagId",
            table: "UserTags",
            column: "TagId");

        migrationBuilder.CreateIndex(
            name: "IX_VacancyTags_TagId",
            table: "VacancyTags",
            column: "TagId");

        migrationBuilder.AddForeignKey(
            name: "FK_Application_User_ApplicantId",
            table: "Application",
            column: "ApplicantId",
            principalTable: "User",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Application_User_ApplicantId",
            table: "Application");

        migrationBuilder.DropTable(
            name: "TimelineEvent");

        migrationBuilder.DropTable(
            name: "UserTags");

        migrationBuilder.DropTable(
            name: "VacancyTags");

        migrationBuilder.DropTable(
            name: "Tags");

        migrationBuilder.DropPrimaryKey(
            name: "PK_User",
            table: "User");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "Vacancy");

        migrationBuilder.DropColumn(
            name: "LastModifiedAt",
            table: "Vacancy");

        migrationBuilder.DropColumn(
            name: "LastModifiedBy",
            table: "Vacancy");

        migrationBuilder.DropColumn(
            name: "TestResult",
            table: "Application");

        migrationBuilder.DropColumn(
            name: "Avatar",
            table: "User");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "User");

        migrationBuilder.DropColumn(
            name: "DateOfBrith",
            table: "User");

        migrationBuilder.DropColumn(
            name: "Discriminator",
            table: "User");

        migrationBuilder.RenameTable(
            name: "User",
            newName: "Applicant");

        migrationBuilder.RenameColumn(
            name: "UpdatedAt",
            table: "Applicant",
            newName: "BirthDay");

        migrationBuilder.RenameColumn(
            name: "SocialMediaHandles",
            table: "Applicant",
            newName: "Patronymic");

        migrationBuilder.AlterColumn<long>(
            name: "CategoryId",
            table: "Vacancy",
            type: "bigint",
            nullable: true,
            oldClrType: typeof(long),
            oldType: "bigint");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Applicant",
            table: "Applicant",
            column: "Id");

        migrationBuilder.CreateTable(
            name: "ApplicationNote",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AplicationId = table.Column<long>(type: "bigint", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApplicationNote", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "ApplicationNote",
            columns: new[] { "Id", "AplicationId", "Date", "Description", "UserId" },
            values: new object[] { 1L, 1L, new DateTime(2023, 5, 2, 11, 6, 53, 299, DateTimeKind.Utc).AddTicks(8887), "", 1L });

        migrationBuilder.UpdateData(
            table: "Vacancy",
            keyColumn: "Id",
            keyValue: 3L,
            column: "CategoryId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Vacancy",
            keyColumn: "Id",
            keyValue: 5L,
            column: "CategoryId",
            value: null);

        migrationBuilder.AddForeignKey(
            name: "FK_Application_Applicant_ApplicantId",
            table: "Application",
            column: "ApplicantId",
            principalTable: "Applicant",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
