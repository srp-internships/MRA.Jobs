using Blazored.TextEditor;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Components.Dialogs;
using BlazorMonaco;
using BlazorMonaco.Editor;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternships;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Client.Pages.Admin.Dialogs;
using MudBlazor;


namespace MRA.Jobs.Client.Pages.Admin;

public partial class InternshipVacanciesPage
{
    private MudTable<InternshipVacancyListResponse> _table;
    private GetInternshipsQueryOptions _query = new();
    
    private string _searchString;

    private async Task<TableData<InternshipVacancyListResponse>> ServerReload(TableState state)
    {
        _query = new()
        {
            Page = state.Page+1,
            PageSize = state.PageSize,
            Filters = GetFilters()
        };
        var result = await InternshipService.GetAll(_query);
        return new TableData<InternshipVacancyListResponse>()
        {
            TotalItems = result.TotalCount,
            Items = result.Items
        };
    }
    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }
    
    private string GetFilters()
    {
        var filters = new List<string>();
        if (!string.IsNullOrEmpty(_searchString)) filters.Add($"Title@={_searchString}");
        return filters.Any() ? string.Join(",", filters) : "";
    }

    private bool _serverError = false;
    private bool _panelOpenState;
    private bool _isInserting;
    private bool _isUpdating = true;
    private string _updateSlug;
    private string _selectedCategory = "";
    private string _newDescription = string.Empty;
    private string _newTitle = string.Empty;
    private List<VacancyQuestionDto> _questions = new();
    private List<CategoryResponse> _categories;
    private List<VacancyTaskDto> _tasks = new();
    private TimeSpan? _applicationDeadlineTime;
    private TimeSpan? _publishDateTime;
    private TimeSpan? _endDateTime;
    private string _createOrEditHeader = "New Internship Vacancy";

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

    protected override async Task OnInitializedAsync()
    {
        await Global.SetTheme(JsRuntime, LayoutService.IsDarkMode ? "vs-dark" : "vs");
        await LoadData();
        Clear();
        StateHasChanged();
    }

    async Task OnDeleteClick(string slug)
    {
        var vacancy = _table.Items.FirstOrDefault(c => c.Slug == slug);
        var parameters = new DialogParameters<DialogMudBlazor>
        {
            { x => x.ContentText, "Do you really want to delete this vacancy?" },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogMudBlazor>("Delete", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var response = await InternshipService.Delete(slug);
            if (response)
            {
                await _table.ReloadServerData();
            }

            Clear();

            StateHasChanged();
        }
    }


    private async Task OnEditClick(string slug)
    {
        var vacancy = await InternshipService.GetBySlug(slug);
        if (vacancy != null)
        {
            _createOrEditHeader = $"Edit {vacancy.Title}";
            InternshipService.createCommand.Title = vacancy.Title;
            InternshipService.createCommand.ShortDescription = vacancy.ShortDescription;
            await _quillHtml.LoadHTMLContent(vacancy.Description);
            InternshipService.createCommand.ApplicationDeadline = vacancy.ApplicationDeadline;
            InternshipService.createCommand.Stipend = vacancy.Stipend;
            InternshipService.createCommand.Duration = vacancy.Duration;
            InternshipService.createCommand.EndDate = vacancy.EndDate;
            InternshipService.createCommand.PublishDate = vacancy.PublishDate;
            InternshipService.createCommand.CategoryId = vacancy.CategoryId;
            _selectedCategory = _categories.FirstOrDefault(c => c.Id == vacancy.CategoryId)?.Name;
            _isInserting = true;
            _isUpdating = false;
            _panelOpenState = true;
            _updateSlug = slug;
            _questions = vacancy.VacancyQuestions.Select(v =>
                new VacancyQuestionDto { Question = v.Question }
            ).ToList();
            _tasks = vacancy.VacancyTasks.Select(v =>
                new VacancyTaskDto
                {
                    Title = v.Title, Description = v.Description, Template = v.Template, Test = v.Test
                }).ToList();
        }
    }

    private void RemoveQuestion(string question)
    {
        var q = _questions.FirstOrDefault(t => t.Question == question);
        _questions.Remove(q);
    }

    private async Task LoadData()
    {
        var categoriesResponse = await CategoryService.GetAllCategory();
        Snackbar.ShowIfError(categoriesResponse, ContentService["ServerIsNotResponding"]);
        if (categoriesResponse.Success)
        {
            _categories = categoriesResponse.Result.Items;
        }
    }


    private void RemoveTask(string title)
    {
        var r = _tasks.FirstOrDefault(t => t.Title == title);
        _tasks.Remove(r);
    }

    BlazoredTextEditor _quillHtml;


    private string _imageLinkToInsertToEditor;

    public async void InsertImage()
    {
        if (!string.IsNullOrEmpty(_imageLinkToInsertToEditor))
        {
            await _quillHtml.InsertImage(_imageLinkToInsertToEditor);
            StateHasChanged();
        }
    }

    private async Task HandleSubmit()
    {
        var catId = _categories.FirstOrDefault(c => c.Name == _selectedCategory)!.Id;
        InternshipService.createCommand.CategoryId = catId;
        InternshipService.createCommand.VacancyQuestions = _questions;
        InternshipService.createCommand.Description = await _quillHtml.GetHTML();

        if (_endDateTime.HasValue)
        {
            DateTime endDate = InternshipService.createCommand.EndDate.Value.Date;
            DateTime newEndDate = endDate.Add(_endDateTime.Value);
            InternshipService.createCommand.EndDate = newEndDate;
        }

        if (_publishDateTime.HasValue)
        {
            DateTime publishDate = InternshipService.createCommand.PublishDate.Value.Date;
            DateTime newPublishDate = publishDate.Add(_publishDateTime.Value);
            InternshipService.createCommand.PublishDate = newPublishDate;
        }

        InternshipService.createCommand.VacancyTasks = _tasks;
        var result = await InternshipService.Create();
        Snackbar.ShowIfError(result, ContentService["ServerIsNotResponding"],
            $"{InternshipService.createCommand.Title} created");

        await LoadData();
        await _table.ReloadServerData();
        Clear();
    }


    private async Task HandleUpdate()
    {
        InternshipService.createCommand.VacancyQuestions = _questions;
        InternshipService.createCommand.VacancyTasks = _tasks;

        InternshipService.createCommand.Description = await _quillHtml.GetHTML();

        var catId = _categories.FirstOrDefault(c => c.Name == _selectedCategory)!.Id;
        InternshipService.createCommand.CategoryId = catId;

        var result = await InternshipService.Update(_updateSlug);
        Snackbar.ShowIfError(result, ContentService["ServerIsNotResponding"], "Updated");

        await LoadData();
        await _table.ReloadServerData();
        Clear();
    }


    private async Task NewQuestionAsync()
    {
        var res =
            (await (await DialogService.ShowAsync<AddQuestionForVacancyDialog>(
                @ContentService["Internships:AddQuestion"])).Result).Data as dynamic;
        Console.WriteLine(res.NewQuestion);
        _questions.Add(new VacancyQuestionDto { Question = res.NewQuestion, IsOptional = res.IsOptional });
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
        _createOrEditHeader = @ContentService["Internships:NewInternship"];
        InternshipService.createCommand.Title = string.Empty;
        InternshipService.createCommand.ShortDescription = string.Empty;
        InternshipService.createCommand.Description = string.Empty;
        InternshipService.createCommand.ApplicationDeadline = DateTime.Now;
        InternshipService.createCommand.Stipend = 0;
        InternshipService.createCommand.Duration = 0;
        InternshipService.createCommand.EndDate = DateTime.Now;
        InternshipService.createCommand.PublishDate = DateTime.Now;
        _isInserting = false;
        _isUpdating = true;
        _panelOpenState = false;
        _updateSlug = string.Empty;
        _questions.Clear();
        _applicationDeadlineTime = DateTime.Now.TimeOfDay;
        _publishDateTime = DateTime.Now.TimeOfDay;
        _endDateTime = DateTime.Now.TimeOfDay;
        _tasks.Clear();
    }

    private async Task OnNoteChangeClick(InternshipVacancyListResponse context)
    {
        await InternshipService.ChangeNoteAsync(context);
    }
    
    private async Task OnTagsClick(InternshipVacancyListResponse vacancyListDto)
    {
        vacancyListDto.Tags =
            await VacanciesService.DialogChangeTagsAsync(vacancyListDto.Id, vacancyListDto.Tags.ToList(),
                vacancyListDto.Title);
        StateHasChanged();
    }
}