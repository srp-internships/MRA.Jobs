namespace MRA.Jobs.Client.Services.HttpClients;

public static class Extensions
{
    public static async Task<T> GetFromJsonAsync<T>(this HttpClient httpClient, string route, object query)
    {
        string queryString = string.Join("&", query.GetType().GetProperties()
            .Select(property => $"{property.Name}={property.GetValue(query)}"));
        return await httpClient.GetFromJsonAsync<T>($"{route}?{queryString}");
    }
}
