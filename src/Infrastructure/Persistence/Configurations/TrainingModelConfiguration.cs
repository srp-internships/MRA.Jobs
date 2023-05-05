using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class TrainingModelConfiguration : IEntityTypeConfiguration<TrainingModel>
{
    public void Configure(EntityTypeBuilder<TrainingModel> builder)
    {
        builder.ToTable(nameof(Vacancy));
    }
}