using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Responses;

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