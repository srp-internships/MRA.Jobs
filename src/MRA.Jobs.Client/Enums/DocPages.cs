namespace MRA.Jobs.Client.Enums;

public static class DocPages
{
    public const string Home = "home";
    public const string Jobs = "/jobs";
    public const string Internships = "/internships";
    public const string Trainings = "/trainings";
    public const string Team = "/team";
    public const string Academy = "/academy";
    public const string Contact = "/contact";
    public const string Profile = "/profile";
    public const string Applications = "/applications";
    public const string NoVacancyUploadCv = "/upload-cv";
    public const string Categories = "/categories";
    public const string UserManager = "/userManager";
    public const string Candidates = "/candidates";
    public const string Users = "/Users";
    public const string EmailTemplates = "/EmailTemplates";

    public static string GetPageFromUrl(string url, bool setDefault = false)
    {
        var allPages = typeof(DocPages).GetFields().Select(s => s.GetValue(null) as string);
        var result = allPages.FirstOrDefault(s =>
            url.Contains(s, StringComparison.OrdinalIgnoreCase) ||
            url.Contains($"dashboard/{s}", StringComparison.OrdinalIgnoreCase));
        if (setDefault || result == null)
            result = Home;

        return result;
    }
}