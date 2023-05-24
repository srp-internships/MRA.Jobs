using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Common properties
        builder.Property(u => u.Avatar).HasColumnType("nvarchar(max)");
        builder.Property(u => u.DateOfBirth).HasColumnType("date");
        builder.Property(u => u.Email).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(u => u.FirstName).HasColumnType("nvarchar(128)").IsRequired();
        builder.Property(u => u.LastName).HasColumnType("nvarchar(128)").IsRequired();
        builder.Property(u => u.PhoneNumber).HasColumnType("nvarchar(32)");

        // Navigation properties
        builder.HasMany(u => u.History)
            .WithOne(ute => ute.User)
            .HasForeignKey(ute => ute.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Tags)
            .WithOne(ut => ut.User)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

