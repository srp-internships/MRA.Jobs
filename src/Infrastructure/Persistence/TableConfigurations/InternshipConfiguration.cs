using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;
public class InternshipConfiguration : IEntityTypeConfiguration<Internship>
{
    public void Configure(EntityTypeBuilder<Internship> builder)
    {
        builder.Property(i => i.ApplicationDeadline).HasColumnType("datetime2");
    }
}
