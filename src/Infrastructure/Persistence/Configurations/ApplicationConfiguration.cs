using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class ApplicationConfiguration : IEntityTypeConfiguration<Domain.Entities.Application>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Application> builder)
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

        builder.HasOne(a => a.Applicant)
            .WithMany(at => at.Applications)
            .HasForeignKey(a => a.ApplicantId);
    }
}
