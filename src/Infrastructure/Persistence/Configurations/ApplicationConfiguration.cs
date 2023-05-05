using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
using Domain.Entities;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ResumeUrl)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.CoverLetter)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.History)
            .HasMaxLength(255);

        builder.HasOne(a => a.Vacancy)
            .WithMany(v => v.Applications)
            .HasForeignKey(a => a.VacancyId);

        builder.HasOne(a => a.Applicant)
            .WithMany(at => at.Applications)
            .HasForeignKey(a => a.ApplicantId);
    }
}
