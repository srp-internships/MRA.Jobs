using Slugify;

namespace MRA.Jobs.Application.Common.SlugGeneratorService;
public class SlugGeneratorService : ISlugGeneratorService
{
    public static readonly SlugGeneratorService Instance = new SlugGeneratorService();
    SlugHelper slugHelper = new SlugHelper();

    private SlugGeneratorService() { }

    public SlugGeneratorService slugGeneratorService
    {
        get
        {
            return Instance;
        }
    }

    public string GenerateSlug(string inputText)
    {
        return slugHelper.GenerateSlug(inputText);
    }
}
