using BlazorMonaco;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components.Web;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Components.Dialogs;
using MRA.Jobs.Client.Identity;
using MRA.Jobs.Client.Pages.Admin.Dialogs;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class VacancyPage
{
    private IEnumerable<JobVacancyListDto> _pagedData;
    private MudTable<JobVacancyListDto> _table;

    private int _totalItems;
    private string _searchString;

    private async Task<TableData<JobVacancyListDto>> ServerReload(TableState state)
    {
        IEnumerable<JobVacancyListDto> data = await VService.GetJobs();
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
            try
            {
                var response = await VService.OnDelete(slug);
                if (response.Success)
                {
                    Snackbar.Add($"Deleted", Severity.Success);
                    _vacancies.Remove(vacancy);
                }
                else
                {
                    Snackbar.Add(response.Error ?? "An error occurred while deleting the job", Severity.Error);
                }
                Clear();
            }
            catch (Exception)
            {
                Snackbar.Add(ContentService["ServerIsNotResponding"], Severity.Error);
            }

            StateHasChanged();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await Global.SetTheme(JsRuntime, LayoutService.IsDarkMode ? "vs-dark" : "vs");
        try
        {
            _category = await VService.GetAllCategory();
            _vacancies = await VService.GetJobs();
        }
        catch (Exception)
        {
            _serverError = true;
            StateHasChanged();
        }
    }

    private async Task HandleSubmit()
    {
        if (_endDateTime.HasValue)
            VService.creatingNewJob.EndDate += _endDateTime.Value;

        if (_publishDateTime.HasValue)
            VService.creatingNewJob.PublishDate += _publishDateTime.Value;

        try
        {
            var catId = _category.First(c => c.Name == _selectedCategory).Id;
            VService.creatingNewJob.CategoryId = catId;
            VService.creatingNewJob.VacancyQuestions = _questions;
            VService.creatingNewJob.VacancyTasks = _tasks;
            var result = await VService.OnSaveCreateClick();
            if (result.Success)
            {
                Snackbar.Add($"{VService.creatingNewJob.Title} created", Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Error ?? "An error occurred while creating the job", Severity.Error);
            }

            await LoadData();
            Clear();
        }
        catch (Exception)
        {
            Snackbar.Add("Server is not responding, please try later", Severity.Error);
        }
    }

    private async Task LoadData()
    {
        _vacancies = await VService.GetJobs();
    }


    public async Task OnEditClick(string slug)
    {
        var vacancy = await VService.GetBySlug(slug);
        _createOrEditHeader = $"Edit {vacancy.Title}";
        VService.creatingNewJob.Title = vacancy.Title;
        VService.creatingNewJob.ShortDescription = vacancy.ShortDescription;
        VService.creatingNewJob.Description = vacancy.Description;
        VService.creatingNewJob.RequiredYearOfExperience = vacancy.RequiredYearOfExperience;
        VService.creatingNewJob.WorkSchedule = vacancy.WorkSchedule;
        if (vacancy.CategoryId.HasValue)
        {
            VService.creatingNewJob.CategoryId = vacancy.CategoryId.Value;
        }

        VService.creatingNewJob.EndDate = vacancy.EndDate;
        VService.creatingNewJob.PublishDate = vacancy.PublishDate;
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
                    Title = v.Title,
                    Description = v.Description,
                    Template = v.Template,
                    Test = v.Test
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
        if (_endDateTime.HasValue)
            VService.creatingNewJob.EndDate += _endDateTime.Value;

        if (_publishDateTime.HasValue)
            VService.creatingNewJob.PublishDate += _publishDateTime.Value;
        VService.creatingNewJob.VacancyQuestions = _questions;
        VService.creatingNewJob.VacancyTasks = _tasks;
        var catId = _category.First(c => c.Name == _selectedCategory).Id;
        VService.creatingNewJob.CategoryId = catId;
        await VService.UpdateJobVacancy(_updateSlug);
        await LoadData();
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

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Code == "Enter")
        {
            await Task.Delay(1);
            AddTask();
        }
    }

    private void HandleCancel()
    {
        Clear();
    }

    private void Clear()
    {
        _createOrEditHeader = "New Job Vacancy";
        VService.creatingNewJob.Title = string.Empty;
        VService.creatingNewJob.ShortDescription = string.Empty;
        VService.creatingNewJob.Description = string.Empty;
        VService.creatingNewJob.RequiredYearOfExperience = 0;
        VService.creatingNewJob.WorkSchedule = 0;
        VService.creatingNewJob.EndDate = DateTime.Now;
        VService.creatingNewJob.PublishDate = DateTime.Now;
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
            (await (await DialogService.ShowAsync<AddQuestionForVacancyDialog>("Add question")).Result).Data as dynamic;
        Console.WriteLine(res.NewQuestion);
        _questions.Add(new VacancyQuestionDto { Question = res.NewQuestion, IsOptional = res.IsOptional });
    }
}