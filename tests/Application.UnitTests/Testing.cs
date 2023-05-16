using AutoMapper.Internal;
using MRA.Jobs.Application.Features.JobVacancies;
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
            cfg.AddProfile<VacancyCategoryProfile>();
        });
        BaseTestFixture.Mapper = configurationProvider.CreateMapper();
    }
}
