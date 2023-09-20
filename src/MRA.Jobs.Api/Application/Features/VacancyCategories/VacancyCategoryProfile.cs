using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.UpdateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.VacancyCategories;

public class VacancyCategoryProfile : Profile
{
    public VacancyCategoryProfile()
    {
        CreateMap<CreateVacancyCategoryCommand, VacancyCategory>();
        CreateMap<UpdateVacancyCategoryCommand, VacancyCategory>();
        CreateMap<VacancyCategory, CategoryResponse>();
    }
}