using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Infrastructure.Persistence.TableConfigurations;
public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.HasKey(us => new { us.UserId, us.SkillId });

        // builder.HasOne(us => us.User)
        //     .WithMany(u => u.UserSkills)
        //     .HasForeignKey(us => us.UserId);
        //
        // builder.HasOne(us => us.Skill)
        //     .WithMany(s => s.UserSkills)
        //     .HasForeignKey(us => us.SkillId);
    }
}