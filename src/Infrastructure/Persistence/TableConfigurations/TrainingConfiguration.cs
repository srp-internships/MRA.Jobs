using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;
public class TrainingConfiguration : IEntityTypeConfiguration<TrainingVacancy>
{
    public void Configure(EntityTypeBuilder<TrainingVacancy> builder)
    {
    }
}