using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.TrainingServices;

public class TrainingService : ITrainingService
{
    private readonly HttpClient _httpClient;

    public TrainingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        createCommand = new CreateTrainingVacancyCommand
        {
            Title = "",
            CategoryId = Guid.NewGuid(),
            Description = "",
            ShortDescription = "",
            Duration = 0,
            EndDate = DateTime.Now,
            PublishDate = DateTime.Now,
            Fees = 0
        };
    }

    public CreateTrainingVacancyCommand createCommand { get; set; }
    public UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    public DeleteTrainingVacancyCommand DeleteCommand { get; set; }

    public async Task<HttpResponseMessage> Create()
    {
        return await _httpClient.PostAsJsonAsync("trainings", createCommand);
    }
    public async Task Delete(string slug)
    {
        await _httpClient.DeleteAsync($"trainings/{slug}");
    }
    public async Task<HttpResponseMessage> Update(string slug)
    {
        UpdateCommand = new UpdateTrainingVacancyCommand
        {
            Slug = slug,
            Title = createCommand.Title,
            Description = createCommand.Description,
            Duration = createCommand.Duration,
            EndDate = createCommand.EndDate,
            PublishDate = createCommand.PublishDate,
            ShortDescription = createCommand.ShortDescription,
            CategoryId = createCommand.CategoryId,
            Fees = createCommand.Fees
        };
        return await _httpClient.PutAsJsonAsync($"trainings/{slug}", UpdateCommand);
    }

    public async Task<PagedList<TrainingVacancyListDto>> GetAll()
    {
        var result = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>("trainings");
        return result;
    }
    public async Task<TrainingVacancyDetailedResponse> GetBySlug(string slug)
    {
        return await _httpClient.GetFromJsonAsync<TrainingVacancyDetailedResponse>($"trainings/{slug}");
    }
    public async Task<List<TrainingCategoriesResponce>> GetCategories()
    {
        var result = await _httpClient.GetFromJsonAsync<List<TrainingCategoriesResponce>>("trainings/categories");
        return result;
    }
    public async Task<PagedList<TrainingVacancyListDto>> GetByCategoryName(string slug)
    {
        var training = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>($"trainings?CategorySlug={slug}");
        return training;
    }
    public async Task<PagedList<TrainingVacancyListDto>> SearchTrainings(string searchInput)
    {
        var trainings = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>($"trainings?SearchText={searchInput}");
        return trainings;
    }

    public async Task<PagedList<TrainingVacancyListDto>> GetAllSinceCheckDate()
    {
        var result = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>("trainings?CheckDate=true");
        return result;
    }
    public async Task<PagedList<TrainingVacancyListDto>> GetByCategorySinceCheckDate(string slug)
    {
        var training = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>($"trainings?CategorySlug={slug}&CheckDate=true");
        return training;
    }
    public async Task<PagedList<TrainingVacancyListDto>> SearchTrainingsSinceSearchDate(string searchInput)
    {
        var trainings = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>($"trainings?SearchText={searchInput}&CheckDate=true");
        return trainings;
    }
    public async Task<List<TrainingCategoriesResponce>> GetCategoriesSinceCheckDate()
    {
        var result = await _httpClient.GetFromJsonAsync<List<TrainingCategoriesResponce>>("trainings/categories?CheckDate=true");
        return result;
    }
}
