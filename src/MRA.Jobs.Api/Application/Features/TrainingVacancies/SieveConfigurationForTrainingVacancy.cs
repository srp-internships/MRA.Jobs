using Sieve.Services;

namespace MRA.Jobs.Application.Features.TrainingVacancies;

public class SieveConfigurationForTrainingVacancy : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<TrainingVacancy>(p => p.Category.Name)
            .CanFilter()
            .HasName(nameof(TrainingVacancy.Category));

        mapper.Property<TrainingVacancy>(p => p.CategoryId)
            .CanFilter();

        mapper.Property<TrainingVacancy>(p => p.Title)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingVacancy>(p => p.ShortDescription)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingVacancy>(p => p.PublishDate)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingVacancy>(p => p.EndDate)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingVacancy>(p => p.Duration)
            .CanFilter()
            .CanSort();

        mapper.Property<TrainingVacancy>(p => p.Fees)
            .CanFilter()
            .CanSort();
    }
}