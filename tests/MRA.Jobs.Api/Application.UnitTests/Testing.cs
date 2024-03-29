using AutoMapper.Internal;
using MRA.Jobs.Application.Features.Applications;
using MRA.Jobs.Application.Features.InternshipVacancies;
using MRA.Jobs.Application.Features.JobVacancies;
using MRA.Jobs.Application.Features.TrainingVacancies;
using MRA.Jobs.Application.Features.VacancyCategories;

namespace MRA.Jobs.Application.UnitTests;

[SetUpFixture]
public class Testing
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        MapperConfiguration configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;
            cfg.AddProfile<JobVacancyProfile>();
            cfg.AddProfile<ApplicationProfile>();
            cfg.AddProfile<InternshipProfile>();
            cfg.AddProfile<VacancyCategoryProfile>();
            cfg.AddProfile<TrainingVacancyProfile>();
        });
        BaseTestFixture.Mapper = configurationProvider.CreateMapper();
    }
}