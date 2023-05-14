using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;
public class TrainingConfiguration : IEntityTypeConfiguration<TrainingModel>
{
    public void Configure(EntityTypeBuilder<TrainingModel> builder)
    {
    }
}