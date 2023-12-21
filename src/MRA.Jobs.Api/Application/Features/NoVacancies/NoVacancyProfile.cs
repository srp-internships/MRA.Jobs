// using MRA.Jobs.Application.Contracts.Dtos.Responses;
// using MRA.Jobs.Application.Contracts.NoVacancies.Responses;
//
// namespace MRA.Jobs.Application.Features.NoVacancies;
//
// public class NoVacancyProfile : Profile
// {
//     public NoVacancyProfile()
//     {
//         CreateMap<VacancyQuestion, VacancyQuestionResponseDto>();
//         CreateMap<NoVacancy, NoVacancyResponse>()
//             .ForMember(dest => dest.VacancyQuestions,
//                 opt => opt
//                     .MapFrom(src => src.VacancyQuestions));
//     }
//
// }