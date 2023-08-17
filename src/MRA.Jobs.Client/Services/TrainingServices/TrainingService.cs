using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.TrainingServices;

public class TrainingService : ITrainingService
{
    private readonly HttpClient _httpClient;
    private readonly ICategoryService _categoryService;

    public TrainingService(HttpClient httpClient, ICategoryService categoryService)
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
        _categoryService = categoryService;

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

    public async Task<List<TrainingVacancyListDto>> GetAll()
    {
        var result = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>("trainings");
        return result.Items;
    }
    public async Task<TrainingVacancyDetailedResponse> GetBySlug(string slug)
    {
        return await _httpClient.GetFromJsonAsync<TrainingVacancyDetailedResponse>($"trainings/{slug}");
    }
    public async Task<List<TrainingVacancyWithCategoryDto>> GetAllWithCategories()
    {
        var result = await _httpClient.GetFromJsonAsync<List<TrainingVacancyWithCategoryDto>>("trainings/getwithcategories");
        return result;
    }
    public async Task<TrainingVacancyWithCategoryDto> GetCategoriesByName(string name)
    {
        var trainings = await GetAllWithCategories();
        var training = trainings.FirstOrDefault(t => t.CategoryName == name);
        return training;
    }
}
