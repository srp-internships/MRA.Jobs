using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class EducationVacancyConfiguration : IEntityTypeConfiguration<EducationVacancy>
{
    public void Configure(EntityTypeBuilder<EducationVacancy> builder)
    {
        // Specific properties
        builder.Property(ev => ev.ClassStartDate).HasColumnType("datetime2");
        builder.Property(ev => ev.ClassEndDate).HasColumnType("datetime2");
    }
}

