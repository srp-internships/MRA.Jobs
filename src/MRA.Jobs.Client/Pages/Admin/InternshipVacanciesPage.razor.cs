using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Components.Dialogs;
using BlazorMonaco;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components.Web;
using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Client.Pages.Admin.Dialogs;
using MudBlazor;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries.GetVacancyCategorySlugId;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;

namespace MRA.Jobs.Client.Pages.Admin;

public partial class InternshipVacanciesPage
{
    private bool _serverError;
    private bool _panelOpenState;
    private bool _isInserting;
    private bool _isUpdating = true;
    private string _updateSlug;
    private string _selectedCategory = "";
    private string _newDescription = string.Empty;
    private string _newTitle = string.Empty;
    private List<VacancyQuestionDto> _questions = new();
    private List<CategoryResponse> _categories;
    private List<InternshipVacancyListResponse> _internships;
    private List<VacancyTaskDto> _tasks = new();
    private GetVacancyCategoryByIdQuery getVacancyCategoryByIdQuery = new();
    private GetInternshipVacancyBySlugQuery getInternshipVacancyBySlugQuery=new();
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
        var vacancy = _internships.FirstOrDefault(c => c.Slug == slug);
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
            try
            {
                var response = await InternshipService.Delete(slug);
                if (response.Success)
                {
                    Snackbar.Add($"Deleted", Severity.Success);
                    _internships.Remove(vacancy);
                }
                else
                {
                    Snackbar.Add(response.Error ?? "An error occurred while deleting the internship", Severity.Error);
                }

                Clear();
            }
            catch (Exception)
            {
                Snackbar.Add("Server is not responding, try later", Severity.Error);
            }

            StateHasChanged();
        }
    }



    private async Task OnEditClick(string slug)
    {
        var vacancy = await InternshipService.GetBySlug(slug,getInternshipVacancyBySlugQuery);
        if (vacancy != null)
        {
            _createOrEditHeader = $"Edit {vacancy.Title}";
            InternshipService.createCommand.Title = vacancy.Title;
            InternshipService.createCommand.ShortDescription = vacancy.ShortDescription;
            InternshipService.createCommand.Description = vacancy.Description;
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
                new VacancyQuestionDto
                {
                    Question = v.Question
                }
            ).ToList();
            _tasks = vacancy.VacancyTasks.Select(v =>
                new VacancyTaskDto
                {
                    Title = v.Title,
                    Description = v.Description,
                    Template = v.Template,
                    Test = v.Test
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
        try
        {
            var internshipsResponse = await InternshipService.GetAll(getInternshipVacancyBySlugQuery);
            if (internshipsResponse.Success)
            {
                _internships = internshipsResponse.Result;
            }
            else
            {
                Snackbar.Add($"Error loading internships: {internshipsResponse.Error}", Severity.Error);
            }

            var categoriesResponse = await CategoryService.GetAllCategory(getVacancyCategoryByIdQuery);
            if (categoriesResponse.Success)
            {
                _categories = categoriesResponse.Result;
            }
            else
            {
                Snackbar.Add($"Error loading categories: {categoriesResponse.Error}", Severity.Error);
            }
        }
        catch (Exception)
        {
            Snackbar.Add("Server is not responding, try later", Severity.Error);
            _serverError = true;
            StateHasChanged();
        }
    }


    private void RemoveTask(string title)
    {
        var r = _tasks.FirstOrDefault(t => t.Title == title);
        _tasks.Remove(r);
    }

    private async Task HandleSubmit()
    {
        var catId = _categories.FirstOrDefault(c => c.Name == _selectedCategory)!.Id;
        InternshipService.createCommand.CategoryId = catId;
        InternshipService.createCommand.VacancyQuestions = _questions;

        if (_applicationDeadlineTime.HasValue)
            InternshipService.createCommand.ApplicationDeadline += _applicationDeadlineTime.Value;

        if (_endDateTime.HasValue)
            InternshipService.createCommand.EndDate += _endDateTime.Value;

        if (_publishDateTime.HasValue)
            InternshipService.createCommand.PublishDate += _publishDateTime.Value;

        try
        {
            InternshipService.createCommand.VacancyTasks = _tasks;
            var result = await InternshipService.Create();
            if (result.Success)
            {
                Snackbar.Add($"{InternshipService.createCommand.Title} created", Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Error ?? "An error occurred while creating the internship", Severity.Error);
            }

            await LoadData();
            Clear();
        }
        catch (Exception)
        {
            Snackbar.Add("Server is not responding, try later", Severity.Error);
        }
    }


    private async Task HandleUpdate()
    {
        InternshipService.createCommand.VacancyQuestions = _questions;
        InternshipService.createCommand.VacancyTasks = _tasks;
        if (_applicationDeadlineTime.HasValue)
            InternshipService.createCommand.ApplicationDeadline += _applicationDeadlineTime.Value;

        if (_endDateTime.HasValue)
            InternshipService.createCommand.EndDate += _endDateTime.Value;

        if (_publishDateTime.HasValue)
            InternshipService.createCommand.PublishDate += _publishDateTime.Value;

        var catId = _categories.FirstOrDefault(c => c.Name == _selectedCategory)!.Id;
        InternshipService.createCommand.CategoryId = catId;

        try
        {
            var result = await InternshipService.Update(_updateSlug);
            if (result.Success)
            {
                Snackbar.Add("Updated", Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Error ?? "An error occurred while updating the internship", Severity.Error);
            }

            await LoadData();
            Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Snackbar.Add("Server is not responding, try later", Severity.Error);
        }
    }


    private async Task NewQuestionAsync()
    {
        var res = (await (await DialogService.ShowAsync<AddQuestionForVacancyDialog>("Add question")).Result).Data as dynamic;
        Console.WriteLine(res.NewQuestion);
        _questions.Add(new VacancyQuestionDto { Question = res.NewQuestion, IsOptional = res.IsOptional});
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
        _createOrEditHeader = "New Internship Vacancy";
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
}