﻿// <auto-generated />
using System;
using MRA.Jobs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MRA.Jobs.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240126112327_TImeLine_Event_change")]
    partial class TImeLine_Event_change
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.ApplicantSocialMedia", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ApplicantSocialMedias");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApplicantUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("AppliedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CV")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverLetter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("VacancyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Slug");

                    b.HasIndex("VacancyId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.EducationDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CertificateDegreeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Major")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UniversityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("EducationDetails");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.ExperienceDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCurrentJob")
                        .HasColumnType("bit");

                    b.Property<string>("JobLocationCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobLocationCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("ExperienceDetails");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Skill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SkillName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.TaskResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TaksId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("TaskResponses");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("NumberOfQuestion")
                        .HasColumnType("bigint");

                    b.Property<int>("PassingScore")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid>("VacancyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("VacancyId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.TestResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Passed")
                        .HasColumnType("bit");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId")
                        .IsUnique();

                    b.HasIndex("TestId");

                    b.ToTable("TestResults");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.TimelineEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreateBy")
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("nvarchar(34)");

                    b.Property<int>("EventType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TimelineEvents");

                    b.HasDiscriminator<string>("Discriminator").HasValue("TimelineEvent");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.UserTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TagId", "UserId")
                        .IsUnique();

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Vacancy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Vacancies");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Vacancy");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsOptional")
                        .HasColumnType("bit");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("VacancyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("VacancyId");

                    b.ToTable("VacancyQuestions");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("VacancyQuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("VacancyQuestionId");

                    b.ToTable("VacancyResponses");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VacancyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TagId");

                    b.HasIndex("VacancyId");

                    b.ToTable("VacancyTags");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Template")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Test")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("VacancyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("VacancyId");

                    b.ToTable("VacancyTasks");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyTaskDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Codes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Log")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Success")
                        .HasColumnType("int");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("VacancyTaskDetails");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.ApplicationTimelineEvent", b =>
                {
                    b.HasBaseType("MRA.Jobs.Domain.Entities.TimelineEvent");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("ApplicationId");

                    b.HasDiscriminator().HasValue("ApplicationTimelineEvent");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.UserTimelineEvent", b =>
                {
                    b.HasBaseType("MRA.Jobs.Domain.Entities.TimelineEvent");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasDiscriminator().HasValue("UserTimelineEvent");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyTimelineEvent", b =>
                {
                    b.HasBaseType("MRA.Jobs.Domain.Entities.TimelineEvent");

                    b.Property<Guid>("VacancyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("VacancyId");

                    b.HasDiscriminator().HasValue("VacancyTimelineEvent");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.InternshipVacancy", b =>
                {
                    b.HasBaseType("MRA.Jobs.Domain.Entities.Vacancy");

                    b.Property<DateTime>("ApplicationDeadline")
                        .HasColumnType("datetime2");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Stipend")
                        .HasColumnType("int");

                    b.ToTable("Vacancies", t =>
                        {
                            t.Property("Duration")
                                .HasColumnName("InternshipVacancy_Duration");
                        });

                    b.HasDiscriminator().HasValue("InternshipVacancy");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.JobVacancy", b =>
                {
                    b.HasBaseType("MRA.Jobs.Domain.Entities.Vacancy");

                    b.Property<int>("RequiredYearOfExperience")
                        .HasColumnType("int");

                    b.Property<int>("WorkSchedule")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("JobVacancy");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.TrainingVacancy", b =>
                {
                    b.HasBaseType("MRA.Jobs.Domain.Entities.Vacancy");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Fees")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("TrainingVacancy");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Application", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Vacancy", "Vacancy")
                        .WithMany("Applications")
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.TaskResponse", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Application", null)
                        .WithMany("TaskResponses")
                        .HasForeignKey("ApplicationId");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Test", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Vacancy", "Vacancy")
                        .WithMany("Tests")
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.TestResult", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Application", "Application")
                        .WithOne("TestResult")
                        .HasForeignKey("MRA.Jobs.Domain.Entities.TestResult", "ApplicationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MRA.Jobs.Domain.Entities.Test", "Test")
                        .WithMany("Results")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.UserTag", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Tag", "Tag")
                        .WithMany("UserTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Vacancy", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.VacancyCategory", "Category")
                        .WithMany("Vacancies")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyQuestion", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Vacancy", null)
                        .WithMany("VacancyQuestions")
                        .HasForeignKey("VacancyId");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyResponse", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Application", null)
                        .WithMany("VacancyResponses")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("MRA.Jobs.Domain.Entities.VacancyQuestion", "VacancyQuestion")
                        .WithMany()
                        .HasForeignKey("VacancyQuestionId");

                    b.Navigation("VacancyQuestion");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyTag", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Tag", "Tag")
                        .WithMany("VacancyTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MRA.Jobs.Domain.Entities.Vacancy", "Vacancy")
                        .WithMany("Tags")
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyTask", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Vacancy", null)
                        .WithMany("VacancyTasks")
                        .HasForeignKey("VacancyId");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.ApplicationTimelineEvent", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Application", "Application")
                        .WithMany("History")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyTimelineEvent", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Vacancy", "Vacancy")
                        .WithMany("History")
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Application", b =>
                {
                    b.Navigation("History");

                    b.Navigation("TaskResponses");

                    b.Navigation("TestResult");

                    b.Navigation("VacancyResponses");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Tag", b =>
                {
                    b.Navigation("UserTags");

                    b.Navigation("VacancyTags");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Test", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Vacancy", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("History");

                    b.Navigation("Tags");

                    b.Navigation("Tests");

                    b.Navigation("VacancyQuestions");

                    b.Navigation("VacancyTasks");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyCategory", b =>
                {
                    b.Navigation("Vacancies");
                });
#pragma warning restore 612, 618
        }
    }
}
