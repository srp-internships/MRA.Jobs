﻿// <auto-generated />
using System;
using MRA.Jobs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MRA.Jobs.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230502093935_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Applicant", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Applicant");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Application", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ApplicantAbout")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<string>("ApplicantCvPath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("ApplicantId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("StatusId")
                        .HasColumnType("bigint");

                    b.Property<long>("VacancyId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.HasIndex("VacancyId");

                    b.ToTable("Application");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.ApplicationNote", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("AplicationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ApplicationNote", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AplicationId = 1L,
                            Date = new DateTime(2023, 5, 2, 9, 39, 35, 560, DateTimeKind.Utc).AddTicks(7640),
                            Description = "",
                            UserId = 1L
                        });
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Vacancy", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("Vacancy", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Vacancy");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.EducationVacancy", b =>
                {
                    b.HasBaseType("MRA.Jobs.Domain.Entities.Vacancy");

                    b.Property<DateTime>("ClassEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ClassStartDate")
                        .HasColumnType("datetime2");

                    b.HasIndex("CategoryId");

                    b.ToTable("Vacancy", (string)null);

                    b.HasDiscriminator().HasValue("EducationVacancy");

                    b.HasData(
                        new
                        {
                            Id = 5L,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "tersd",
                            EndDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ShortDescription = "sad",
                            Title = "Training class",
                            ClassEndDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ClassStartDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.JobVacancy", b =>
                {
                    b.HasBaseType("MRA.Jobs.Domain.Entities.Vacancy");

                    b.Property<int>("RequiredYearOfExperience")
                        .HasColumnType("int");

                    b.Property<int>("WorkSchedule")
                        .HasColumnType("int");

                    b.HasIndex("CategoryId");

                    b.ToTable("Vacancy", (string)null);

                    b.HasDiscriminator().HasValue("JobVacancy");

                    b.HasData(
                        new
                        {
                            Id = 3L,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "tersd",
                            EndDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ShortDescription = "sad",
                            Title = "Senior C# backend developer",
                            RequiredYearOfExperience = 0,
                            WorkSchedule = 0
                        });
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Application", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.Applicant", "Applicant")
                        .WithMany("Applications")
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MRA.Jobs.Domain.Entities.Vacancy", "Vacancy")
                        .WithMany()
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Applicant");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.EducationVacancy", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.VacancyCategory", "Category")
                        .WithMany("EducationVacancies")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.JobVacancy", b =>
                {
                    b.HasOne("MRA.Jobs.Domain.Entities.VacancyCategory", "Category")
                        .WithMany("JobVacancies")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.Applicant", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("MRA.Jobs.Domain.Entities.VacancyCategory", b =>
                {
                    b.Navigation("EducationVacancies");

                    b.Navigation("JobVacancies");
                });
#pragma warning restore 612, 618
        }
    }
}
