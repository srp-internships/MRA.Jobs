using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class ApplicantSocialMediaConfiguration : IEntityTypeConfiguration<ApplicantSocialMedia>
{
    public void Configure(EntityTypeBuilder<ApplicantSocialMedia> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.Property(sm => sm.ProfileUrl).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(sm => sm.Type).HasColumnType("int").IsRequired();

        // Navigation properties
        builder.HasOne(sm => sm.Applicant)
            .WithMany(a => a.SocialMedias)
            .HasForeignKey(sm => sm.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
       
    }
}

