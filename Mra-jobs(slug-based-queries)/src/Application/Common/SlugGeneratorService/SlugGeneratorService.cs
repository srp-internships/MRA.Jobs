using Slugify;

namespace MRA.Jobs.Application.Common.SlugGeneratorService;
public class SlugGeneratorService : ISlugGeneratorService
{
    static readonly SlugHelper slugHelper = new SlugHelper();

    public string GenerateSlug(string inputText)
    {
        return slugHelper.GenerateSlug(inputText);
    }
}
