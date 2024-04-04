using Blazored.TextEditor;
using BlazorMonaco;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components.Web;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Components.Dialogs;
using MudBlazor;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class TrainingVacanciesPage
{
    private GetTrainingsQueryOptions _query = new();
    private MudTable<TrainingVacancyListDto> _table;
    
    private string _searchString;

    private async Task<TableData<TrainingVacancyListDto>> ServerReload(TableState state)
    {
        _query = new()
        {
            Page = state.Page+1,
            PageSize = state.PageSize,
            Filters = GetFilters()
        };
        var result = await TrainingService.GetAll(_query);
        return new TableData<TrainingVacancyListDto>()
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
    private string _selectedCategory = string.Empty;
    private string _newDescription = string.Empty;
    private string _newTitle = string.Empty;
    private string _createOrEditHeader = "New Training Vacancy";
    private List<VacancyQuestionDto> _questions = new();
    private List<CategoryResponse> _categories;
    private List<VacancyTaskDto> _tasks = new();
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
        else
        {
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
        await LoadData();
        await Global.SetTheme(JsRuntime, LayoutService.IsDarkMode ? "vs-dark" : "vs");
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
            var response = await TrainingService.Delete(slug);
            Snackbar.ShowIfError(response, ContentService["ServerIsNotResponding"], "Deleted");
            if (response.Success)
            {
                await _table.ReloadServerData();
            }

            Clear();

            StateHasChanged();
        }
    }


    private async Task OnEditClick(string slug)
    {
        var vacancy = await TrainingService.GetBySlug(slug);
        if (vacancy != null)
        {
            _createOrEditHeader = $"Edit {vacancy.Title}";
            TrainingService.createCommand.Title = vacancy.Title;
            TrainingService.createCommand.ShortDescription = vacancy.ShortDescription;
            await _quillHtml.LoadHTMLContent(vacancy.Description);
            TrainingService.createCommand.Duration = vacancy.Duration;
            TrainingService.createCommand.EndDate = vacancy.EndDate;
            TrainingService.createCommand.PublishDate = vacancy.PublishDate;
            TrainingService.createCommand.CategoryId = vacancy.CategoryId;
            TrainingService.createCommand.Fees = vacancy.Fees;
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

    private void RemoveTask(string title)
    {
        var r = _tasks.FirstOrDefault(t => t.Title == title);
        _tasks.Remove(r);
    }

    private async Task LoadData()
    { 
        var categoriesResponse = await CategoryService.GetAllCategory();
        Snackbar.ShowIfError(categoriesResponse, ContentService["ServerIsNotResponding"]);

        if (categoriesResponse.Success)
        {
            _categories = categoriesResponse.Result.Items;
            StateHasChanged();
        }
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
        if (_endDateTime.HasValue)
        {
            DateTime endDate = TrainingService.createCommand.EndDate.Value.Date;
            DateTime newEndDate = endDate.Add(_endDateTime.Value);
            TrainingService.createCommand.EndDate = newEndDate;
        }

        if (_publishDateTime.HasValue)
        {
            DateTime publishDate = TrainingService.createCommand.PublishDate.Value.Date;
            DateTime newPublishDate = publishDate.Add(_publishDateTime.Value);
            TrainingService.createCommand.PublishDate = newPublishDate;
        }

        var catId = _categories.FirstOrDefault(c => c.Name == _selectedCategory)!.Id;
        TrainingService.createCommand.CategoryId = catId;
        TrainingService.createCommand.VacancyQuestions = _questions;
        TrainingService.createCommand.VacancyTasks = _tasks;
        TrainingService.createCommand.Description = await _quillHtml.GetHTML();
        var result = await TrainingService.Create();
        Snackbar.ShowIfError(result, ContentService["ServerIsNotResponding"],
            $"{TrainingService.createCommand.Title} created");

        if (result.Success)
        {
            Snackbar.Add($"{TrainingService.createCommand.Title} created", Severity.Success);
        }

        await LoadData();
        await _table.ReloadServerData();
        Clear();
    }

    private async Task HandleUpdate()
    {
        if (_endDateTime.HasValue)
            TrainingService.createCommand.EndDate += _endDateTime.Value;

        if (_publishDateTime.HasValue)
            TrainingService.createCommand.PublishDate += _publishDateTime.Value;
        TrainingService.createCommand.VacancyQuestions = _questions;
        TrainingService.createCommand.VacancyTasks = _tasks;
        TrainingService.createCommand.Description = await _quillHtml.GetHTML();
        var catId = _categories.FirstOrDefault(c => c.Name == _selectedCategory)!.Id;
        TrainingService.createCommand.CategoryId = catId;
        try
        {
            var result = await TrainingService.Update(_updateSlug);
            Snackbar.ShowIfError(result, ContentService["ServerIsNotResponding"], "Updated");

            if (result.Success)
            {
                Snackbar.Add("Updated", Severity.Success);
            }

            await LoadData();
            await _table.ReloadServerData();
            Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Snackbar.Add(ContentService["ServerIsNotResponding"], Severity.Error);
        }
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

    void HandleCancel()
    {
        Clear();
    }

    private void Clear()
    {
        _createOrEditHeader = @ContentService["Training:NewTrainingV"];
        TrainingService.createCommand.Title = string.Empty;
        TrainingService.createCommand.ShortDescription = string.Empty;
        TrainingService.createCommand.Description = string.Empty;
        TrainingService.createCommand.Fees = 0;
        TrainingService.createCommand.Duration = 0;
        TrainingService.createCommand.EndDate = DateTime.Now;
        TrainingService.createCommand.PublishDate = DateTime.Now;
        _isInserting = false;
        _isUpdating = true;
        _panelOpenState = false;
        _updateSlug = string.Empty;
        _questions.Clear();
        _tasks.Clear();
    }

    private async Task NewQuestionAsync()
    {
        var res =
            (await (await DialogService.ShowAsync<AddQuestionForVacancyDialog>(
                @ContentService["Internships:AddQuestion"])).Result).Data as dynamic;
        Console.WriteLine(res.NewQuestion);
        _questions.Add(new VacancyQuestionDto { Question = res.NewQuestion, IsOptional = res.IsOptional });
    }

    private async Task OnNoteChangeClick(TrainingVacancyListDto context)
    {
        await TrainingService.ChangeNoteAsync(context);
    }
    
    private async Task OnTagsClick(TrainingVacancyListDto vacancyListDto)
    {
        vacancyListDto.Tags =
            await VacanciesService.DialogChangeTagsAsync(vacancyListDto.Id, vacancyListDto.Tags.ToList(),
                vacancyListDto.Title);
        StateHasChanged();
    }
}