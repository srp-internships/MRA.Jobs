using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Contracts.TrainingModels.Responses;

namespace MRA.Jobs.Application.Features.TrainingModels;
public class TrainingModelProfile : Profile
{
    public TrainingModelProfile()
    {
        CreateMap<TrainingModel, TrainingModelListDTO>();
        CreateMap<TrainingModel, TrainingModelDetailsDTO>();
        CreateMap<CreateTrainingModelCommand, TrainingModel>();
        CreateMap<UpdateTrainingModelCommand, TrainingModel>();
    }
}
