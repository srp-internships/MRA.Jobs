using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class JobVacancyConfiguration : IEntityTypeConfiguration<JobVacancy>
{
    public void Configure(EntityTypeBuilder<JobVacancy> builder)
    {
        // Specific properties
        builder.Property(jv => jv.RequiredYearOfExperience).HasColumnType("int");
        builder.Property(jv => jv.WorkSchedule).HasColumnType("int");
    }
}