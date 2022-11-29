using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BFF.Component.Tests.Support;

public static class Extensions
{
    public static T ContentAs<T>(this HttpResponseMessage response) => response.Content.ReadFromJsonAsync<T>().Result;
    public static T AsDeserializedJson<T>(this string body) => JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
}