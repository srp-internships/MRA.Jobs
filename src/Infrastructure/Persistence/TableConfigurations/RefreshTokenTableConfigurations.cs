using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Infrastructure.Identity.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class RefreshTokenTableConfigurations : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(nameof(RefreshToken));
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(u => u.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}