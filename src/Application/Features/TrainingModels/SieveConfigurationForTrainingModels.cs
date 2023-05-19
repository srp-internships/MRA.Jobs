using Sieve.Services;

namespace MRA.Jobs.Application.Features.TrainingModels;
public class SieveConfigurationForTrainingModels : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<TrainingModel>(p => p.Category.Name)
            .CanFilter()
            .HasName(nameof(TrainingModel.Category));

        mapper.Property<TrainingModel>(p => p.CategoryId)
            .CanFilter();

        mapper.Property<TrainingModel>(p => p.Title)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingModel>(p => p.ShortDescription)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingModel>(p => p.PublishDate)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingModel>(p => p.EndDate)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingModel>(p => p.Duration)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingModel>(p => p.Fees)
            .CanFilter()
            .CanSort();
    }
}
