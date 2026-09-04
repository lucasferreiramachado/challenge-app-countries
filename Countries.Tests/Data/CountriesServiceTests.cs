using Core.Services.Http;
using Core.Services.Storage.Settings.Models;
using Countries.Services;
using Xunit;

namespace Countries.Tests.Data;

public sealed class CountriesServiceTests
{
    [Fact]
    public async Task GetCountriesByRegionAsync_ShouldMapApiResponseToCountryModels()
    {
        var service = new CountryService(new StubHttpClient(CreateCountryData()));
        var result = await service.GetCountriesByRegionAsync(AppRegion.SouthAmerica);

        Assert.Equal(2, result.Count);
        Assert.Equal("Brasil", result[0].Name);
        Assert.Equal("https://example.com/brasil.png", result[0].FlagUrl);
        Assert.Equal("Argentina", result[1].Name);
    }

    private static CountryService.CountryData CreateCountryData() => new()
    {
        Data = new CountryService.CountryObjects
        {
            Objects =
            [
                CreateCountry("Brasil", "https://example.com/brasil.png"),
                CreateCountry("Argentina", "https://example.com/argentina.png")
            ]
        }
    };

    private static CountryService.CountryResponse CreateCountry(string name, string flagUrl) => new()
    {
        Names = new CountryService.CountryNames
        {
            Translations = new CountryService.CountryTranslations
            {
                Portuguese = new CountryService.CountryTranslation { Common = name }
            }
        },
        Flag = new CountryService.CountryFlag { UrlPng = flagUrl }
    };

    private sealed class StubHttpClient(CountryService.CountryData response) : IHttpClient
    {
        public Task<T> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default)
            => typeof(T) == typeof(CountryService.CountryData)
                ? Task.FromResult((T)(object)response)
                : throw new InvalidOperationException("Unexpected type");
    }
}
