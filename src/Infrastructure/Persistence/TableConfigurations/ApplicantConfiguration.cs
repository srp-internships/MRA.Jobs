using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
{
    public void Configure(EntityTypeBuilder<Applicant> builder)
    {
        // Navigation properties
        builder.HasMany(a => a.Applications)
            .WithOne(ap => ap.Applicant)
            .HasForeignKey(ap => ap.ApplicantId);

        builder.HasMany(a => a.SocialMedias)
            .WithOne(sm => sm.Applicant)
            .HasForeignKey(sm => sm.ApplicantId);
    }
}

