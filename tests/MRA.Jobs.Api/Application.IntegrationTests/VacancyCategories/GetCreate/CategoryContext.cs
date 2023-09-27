using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
public class CategoryContext : Testing
{
    public async Task<Guid> GetCategoryId(string name)
    {
        var catogy = await FindFirstOrDefaultAsync<VacancyCategory>(c => c.Name == name);
        if (catogy != null)
            return catogy.Id;

        var newCategory = new VacancyCategory
        {
            Name = name,
            Slug = name.ToLower().Replace(" ", "-")
        };
        await AddAsync(newCategory);
        return newCategory.Id;
    }

    public async Task<VacancyCategory> GetCategory(string name)
    {
        var catogy = await FindFirstOrDefaultAsync<VacancyCategory>(c => c.Name == name);
        if (catogy != null)
            return catogy;

        var newCategory = new VacancyCategory
        {
            Name = name,
            Slug = name.ToLower().Replace(" ", "-")
        };
        await AddAsync(newCategory);
        return newCategory;
    }
}
