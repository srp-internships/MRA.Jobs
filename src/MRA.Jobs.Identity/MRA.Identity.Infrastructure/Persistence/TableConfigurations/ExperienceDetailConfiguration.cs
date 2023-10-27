using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Infrastructure.Persistence.TableConfigurations;

public class ExperienceDetailConfiguration : IEntityTypeConfiguration<ExperienceDetail>
{
    public void Configure(EntityTypeBuilder<ExperienceDetail> builder)
    {
        // builder.HasOne(e => e.User)
        //     .WithMany(u => u.Experiences)
        //     .HasForeignKey(e => e.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}
