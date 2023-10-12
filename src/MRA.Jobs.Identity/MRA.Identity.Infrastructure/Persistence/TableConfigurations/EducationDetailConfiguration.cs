using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Infrastructure.Persistence.TableConfigurations;

public class EducationDetailConfiguration : IEntityTypeConfiguration<EducationDetail>
{
    public void Configure(EntityTypeBuilder<EducationDetail> builder)
    {
        builder.HasOne(e => e.User)
            .WithMany(u => u.Educations)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
