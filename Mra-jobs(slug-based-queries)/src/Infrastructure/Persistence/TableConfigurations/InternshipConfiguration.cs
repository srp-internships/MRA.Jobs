using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;
public class InternshipConfiguration : IEntityTypeConfiguration<InternshipVacancy>
{
    public void Configure(EntityTypeBuilder<InternshipVacancy> builder)
    {
      
        builder.Property(i => i.ApplicationDeadline).HasColumnType("datetime2");
    }
}
