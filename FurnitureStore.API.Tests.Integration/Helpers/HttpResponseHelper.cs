using Newtonsoft.Json;

namespace FurnitureStore.API.Tests.Integration.Helpers;

public static class HttpResponseHelper
{
    public static async Task<T?> GetFromResponse<T>(HttpResponseMessage response)
    {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        T? t = JsonConvert.DeserializeObject<T>(jsonResponse);
        return t;
    }

    public static async Task<T?> ReadFromResponse<T>(HttpResponseMessage response)
    {
        if (response.Content is null)
            return default(T);

        return await response.Content.ReadFromJsonAsync<T>();
    }
}
