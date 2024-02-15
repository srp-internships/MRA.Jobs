using Blazored.TextEditor;
using BlazorMonaco;
using BlazorMonaco.Editor;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Components.Dialogs;
using MRA.Jobs.Client.Pages.Admin.Dialogs;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class JobVacanciesPage
{
    private IEnumerable<JobVacancyListDto> _pagedData;
    private MudTable<JobVacancyListDto> _table;

    private int _totalItems;
    private string _searchString;

    private async Task<TableData<JobVacancyListDto>> ServerReload(TableState state)
    {
        IEnumerable<JobVacancyListDto> data = await JobsService.GetJobs();
        await Task.Delay(100);
        data = data.Where(element =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (element.Title.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }).ToArray();
        _totalItems = data.Count();

        _pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new TableData<JobVacancyListDto>() { TotalItems = _totalItems, Items = _pagedData };
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }


    private string _createOrEditHeader = "New Job vacancy";
    private bool _serverError;

    private readonly ApplicationStatusDto.WorkSchedule[] _value2Items = Enum
        .GetValues(typeof(ApplicationStatusDto.WorkSchedule)).Cast<ApplicationStatusDto.WorkSchedule>().ToArray();

    BlazoredTextEditor QuillHtml;


    private string ImageLinkToInsertToEditor;

    public async void InsertImage()
    {
        if (!string.IsNullOrEmpty(ImageLinkToInsertToEditor))
        {
            await this.QuillHtml.InsertImage(ImageLinkToInsertToEditor);
            StateHasChanged();
        }
    }

    private List<CategoryResponse> _category;
    private List<JobVacancyListDto> _vacancies;
    private bool _panelOpenState;
    private bool _isInserting;
    private bool _isUpdating = true;
    private string _updateSlug;
    private string _selectedCategory = string.Empty;
    private string _newDescription = string.Empty;
    private string _newTitle = string.Empty;
    private List<VacancyTaskDto> _tasks = new();

    private List<VacancyQuestionDto> _questions = new();

    private TimeSpan? _publishDateTime;
    private TimeSpan? _endDateTime;
    private StandaloneCodeEditor _editorTemplate = null!;
    private StandaloneCodeEditor _editorTest = null!;

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        if (editor == _editorTest)
        {
            return new StandaloneEditorConstructionOptions
            {
                Language = "csharp",
                GlyphMargin = true,
                AutomaticLayout = true,
                Value = "using Microsoft.VisualStudio.TestTools.UnitTesting;\n\n" +
                        "namespace TestProject\n" +
                        "{\n" +
                        "    [TestClass]\n" +
                        "    public class UnitTest1\n" +
                        "    {\n" +
                        "        [TestMethod]\n" +
                        "        public void TestMethod1()\n" +
                        "        {\n" +
                        "        //You can write a test here  \n" +
                        "        }\n" +
                        "    }\n" +
                        "}\n"
            };
        }

        return new StandaloneEditorConstructionOptions
        {
            Language = "csharp",
            GlyphMargin = true,
            AutomaticLayout = true,
            Value = "using System;\n\n" +
                    "public class Program\n" +
                    "{\n" +
                    "    public static void Main()\n" +
                    "    {\n" +
                    "        Console.WriteLine(\"Hello, world!\");\n" +
                    "    }\n" +
                    "}\n"
        };
    }

    private async Task EditorOnDidInit()
    {
        await _editorTest.AddCommand((int)KeyMod.CtrlCmd | (int)KeyCode.KeyH,
            _ => { Console.WriteLine(@"Ctrl+H : Initial editor command is triggered."); });
        await _editorTemplate.AddCommand((int)KeyMod.CtrlCmd | (int)KeyCode.KeyH,
            _ => { Console.WriteLine(@"Ctrl+H : Initial editor command is triggered."); });

        var newDecorations = new[]
        {
            new ModelDeltaDecoration
            {
                Range = new BlazorMonaco.Range(3, 1, 3, 1),
                Options = new ModelDecorationOptions
                {
                    IsWholeLine = true,
                    ClassName = "decorationContentClass",
                    GlyphMarginClassName = "decorationGlyphMarginClass"
                }
            }
        };

        await _editorTest.DeltaDecorations(null, newDecorations);
        await _editorTemplate.DeltaDecorations(null, newDecorations);
    }

    private void OnContextMenu(EditorMouseEvent eventArg)
    {
        Console.WriteLine(@"OnContextMenu : " + System.Text.Json.JsonSerializer.Serialize(eventArg));
    }

    async Task OnDeleteClick(string slug)
    {
        var vacancy = _vacancies.FirstOrDefault(c => c.Slug == slug);
        var parameters = new DialogParameters<DialogMudBlazor>
        {
            { x => x.ContentText, "Do you really want to delete this vacancy?" },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogMudBlazor>("Delete", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var response = await JobsService.OnDelete(slug);
            Snackbar.ShowIfError(response, ContentService["ServerIsNotResponding"], "Deleted");

            if (response.Success)
            {
                _vacancies.Remove(vacancy);
            }

            Clear();

            StateHasChanged();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await Global.SetTheme(JsRuntime, LayoutService.IsDarkMode ? "vs-dark" : "vs");
        await LoadData();
        Clear();
        StateHasChanged();
    }

    private async Task HandleSubmit()
    {
        JobsService.creatingNewJob.Description = await this.QuillHtml.GetHTML();

        if (_endDateTime.HasValue)
        {
            DateTime endDate = JobsService.creatingNewJob.EndDate.Value.Date;
            DateTime newEndDate = endDate.Add(_endDateTime.Value);
            JobsService.creatingNewJob.EndDate = newEndDate;
        }

        if (_publishDateTime.HasValue)
        {
            DateTime publishDate = JobsService.creatingNewJob.PublishDate.Value.Date;
            DateTime newPublishDate = publishDate.Add(_publishDateTime.Value);
            JobsService.creatingNewJob.PublishDate = newPublishDate;
        }

        var catId = _category.First(c => c.Name == _selectedCategory).Id;
        JobsService.creatingNewJob.CategoryId = catId;
        JobsService.creatingNewJob.VacancyQuestions = _questions;
        JobsService.creatingNewJob.VacancyTasks = _tasks;
        var result = await JobsService.OnSaveCreateClick();
        Snackbar.ShowIfError(result, ContentService["ServerIsNotResponding"],
            $"{JobsService.creatingNewJob.Title} created");

        await LoadData();
        await _table.ReloadServerData();
        Clear();
        Snackbar.Add(ContentService["ServerIsNotResponding"], Severity.Error);
    }

    private async Task LoadData()
    {
        try
        {
            _category = await JobsService.GetAllCategory() ?? throw new Exception("Category fetch failed");
            _vacancies = await JobsService.GetJobs() ?? throw new Exception("Job fetch failed");
        }
        catch (Exception)
        {
            Snackbar.Add(ContentService["ServerIsNotResponding"], Severity.Error);
            _serverError = true;
            StateHasChanged();
        }
    }


    public async Task OnEditClick(string slug)
    {
        var vacancy = await JobsService.GetBySlug(slug);
        _createOrEditHeader = $"Edit {vacancy.Title}";
        JobsService.creatingNewJob.Title = vacancy.Title;
        JobsService.creatingNewJob.ShortDescription = vacancy.ShortDescription;
        await this.QuillHtml.LoadHTMLContent(vacancy.Description);
        JobsService.creatingNewJob.RequiredYearOfExperience = vacancy.RequiredYearOfExperience;
        JobsService.creatingNewJob.WorkSchedule = vacancy.WorkSchedule;
        if (vacancy.CategoryId.HasValue)
        {
            JobsService.creatingNewJob.CategoryId = vacancy.CategoryId.Value;
        }

        JobsService.creatingNewJob.EndDate = vacancy.EndDate;
        JobsService.creatingNewJob.PublishDate = vacancy.PublishDate;
        _selectedCategory = _category.First(c => c.Id == vacancy.CategoryId).Name;
        _panelOpenState = true;
        _isInserting = true;
        _isUpdating = false;
        _updateSlug = slug;
        _questions = vacancy.VacancyQuestions.Select(v =>
            new VacancyQuestionDto { Question = v.Question }
        ).ToList();
        _tasks = vacancy.VacancyTasks.Select(v =>
                new VacancyTaskDto
                {
                    Title = v.Title, Description = v.Description, Template = v.Template, Test = v.Test
                })
            .ToList();
    }

    private void RemoveQuestion(string question)
    {
        var q = _questions.FirstOrDefault(t => t.Question == question);
        _questions.Remove(q);
    }

    private void RemoveTask(string title)
    {
        var r = _tasks.FirstOrDefault(t => t.Title == title);
        _tasks.Remove(r);
    }

    private async Task HandleUpdate()
    {
        JobsService.creatingNewJob.Description = await this.QuillHtml.GetHTML();

        if (_endDateTime.HasValue)
            JobsService.creatingNewJob.EndDate += _endDateTime.Value;

        if (_publishDateTime.HasValue)
            JobsService.creatingNewJob.PublishDate += _publishDateTime.Value;
        JobsService.creatingNewJob.VacancyQuestions = _questions;
        JobsService.creatingNewJob.VacancyTasks = _tasks;
        var catId = _category.First(c => c.Name == _selectedCategory).Id;
        JobsService.creatingNewJob.CategoryId = catId;
        await JobsService.UpdateJobVacancy(_updateSlug);
        await LoadData();
        await _table.ReloadServerData();
        Clear();
    }

    private async void AddTask()
    {
        _tasks.Add(new VacancyTaskDto
        {
            Title = _newTitle,
            Description = _newDescription,
            Template = await _editorTemplate.GetValue(),
            Test = await _editorTest.GetValue()
        });
        _newTitle = "";
        _newDescription = "";
    }

    private void HandleCancel()
    {
        Clear();
    }

    private void Clear()
    {
        _createOrEditHeader = @ContentService["job:NewVacancy"];
        JobsService.creatingNewJob.Title = string.Empty;
        JobsService.creatingNewJob.ShortDescription = string.Empty;
        JobsService.creatingNewJob.Description = string.Empty;
        JobsService.creatingNewJob.RequiredYearOfExperience = 0;
        JobsService.creatingNewJob.WorkSchedule = 0;
        JobsService.creatingNewJob.EndDate = DateTime.Now;
        JobsService.creatingNewJob.PublishDate = DateTime.Now;
        _isInserting = false;
        _isUpdating = true;
        _updateSlug = string.Empty;
        _questions.Clear();
        _tasks.Clear();
        _panelOpenState = false;
    }

    private async Task NewQuestionAsync()
    {
        var res =
            (await (await DialogService.ShowAsync<AddQuestionForVacancyDialog>(
                @ContentService["Internships:AddQuestion"])).Result).Data as dynamic;
        Console.WriteLine(res.NewQuestion);
        _questions.Add(new VacancyQuestionDto { Question = res.NewQuestion, IsOptional = res.IsOptional });
    }

    private async Task OnNoteChangeClick(JobVacancyListDto vacancy)
    {
        await JobsService.ChangeNoteAsync(vacancy);
    }

    private async Task OnTagsClick(JobVacancyListDto vacancyListDto)
    {
        vacancyListDto.Tags =
            await VacncyServise.DialogChangeTagsAsync(vacancyListDto.Id, vacancyListDto.Tags.ToList(),
                vacancyListDto.Title);
        StateHasChanged();
    }
}