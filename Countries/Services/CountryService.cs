
using System.Text.Json.Serialization;
using Core.Services.Http;
using Core.Services.Storage.Settings.Models;

namespace Countries.Services;

public class CountryService(IHttpClient httpClient)
{
    public async Task<IReadOnlyList<Country>> GetCountriesByRegionAsync(AppRegion region, CancellationToken cancellationToken = default)
    {
        var subregion = Uri.EscapeDataString(region.ApiSubregion());
        var url = $"countries/v5?region=Americas&subregion={subregion}&response_fields=names.translations.por.common,flag.url_png&response_fields_omit=_match,_meta&&pretty=1";
        var response = await httpClient.GetAsync<CountryData>(url, cancellationToken);

        return response.Data.Objects
            .Where(country => !string.IsNullOrWhiteSpace(country.Names?.Translations?.Portuguese?.Common))
            .Select(country => new Country(
                country.Names!.Translations!.Portuguese!.Common!,
                country.Flag?.UrlPng ?? string.Empty))
            .ToArray() ?? [];
    }
    
    public sealed class CountryData
    {
        [JsonPropertyName("data")] public CountryObjects? Data { get; set; }
    }
    
    public sealed class CountryObjects
    {
        [JsonPropertyName("objects")] public List<CountryResponse>? Objects { get; set; }
    }

    public sealed class CountryResponse
    {
        [JsonPropertyName("names")] public CountryNames? Names { get; set; }
        [JsonPropertyName("flag")] public CountryFlag? Flag { get; set; }
    }

    public sealed class CountryNames
    {
        [JsonPropertyName("translations")] public CountryTranslations? Translations { get; set; }
    }

    public sealed class CountryTranslations
    {
        [JsonPropertyName("por")] public CountryTranslation? Portuguese { get; set; }
    }

    public sealed class CountryTranslation
    {
        [JsonPropertyName("common")] public string? Common { get; set; }
    }

    public sealed class CountryFlag
    {
        [JsonPropertyName("url_png")] public string? UrlPng { get; set; }
    }
}
