using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
using Domain.Entities;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ApplicantCvPath)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.ApplicantAbout)
            .HasMaxLength(3000)
            .IsRequired();

        /*builder.HasOne(a => a.Applicant)
            .WithMany(at => at.Applications)
            .HasForeignKey(a => a.ApplicantId);*/
    }
}
