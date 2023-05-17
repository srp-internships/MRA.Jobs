using AutoMapper.Internal;
using MRA.Jobs.Application.Features.Applications;
using MRA.Jobs.Application.Features.Internships;
using MRA.Jobs.Application.Features.JobVacancies;
using MRA.Jobs.Application.Features.TrainingModels;
using MRA.Jobs.Application.Features.VacancyCategories;

namespace MRA.Jobs.Application.UnitTests;

[SetUpFixture]
public partial class Testing
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;
            cfg.AddProfile<JobVacancyProfile>();
            cfg.AddProfile<ApplicationProfile>();
            cfg.AddProfile<InternshipProfile>();
            cfg.AddProfile<VacancyCategoryProfile>();
            cfg.AddProfile<TrainingModelProfile>();
        });
        BaseTestFixture.Mapper = configurationProvider.CreateMapper();
    }
}
