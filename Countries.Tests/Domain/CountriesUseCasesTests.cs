using Core.Services.Storage.Settings;
using Core.Services.Storage.Settings.Models;
using Countries.Domain.UseCases.Countries;
using Xunit;

namespace Countries.Tests.Domain;

public sealed class CountriesUseCasesTests
{
    [Fact]
    public void SaveSelectedCountriesByRegionUseCase_ShouldPersistCountriesForRegion()
    {
        var storage = new InMemoryStorage();
        new SaveSelectedCountriesByRegionUseCase(storage).Execute(nameof(AppRegion.NorthAmerica), ["Canada", "United States"]);

        Assert.Equal(["Canada", "United States"],
            new GetSelectedCountriesByRegionUseCase(storage).Execute(nameof(AppRegion.NorthAmerica)));
    }

    [Fact]
    public void RemoveSelectedCountryByRegionUseCase_ShouldRemoveCountryFromSelection()
    {
        var storage = new InMemoryStorage();
        new SaveSelectedCountriesByRegionUseCase(storage).Execute(nameof(AppRegion.SouthAmerica), ["Brasil", "Argentina", "Uruguai"]);
        new RemoveSelectedCountryByRegionUseCase(storage).Execute(nameof(AppRegion.SouthAmerica), "Argentina");

        Assert.Equal(["Brasil", "Uruguai"],
            new GetSelectedCountriesByRegionUseCase(storage).Execute(nameof(AppRegion.SouthAmerica)));
    }

    [Fact]
    public void SaveEmptySelectedCountriesByRegionUseCase_ShouldPersistEmptySelection()
    {
        var storage = new InMemoryStorage();
        new SaveEmptySelectedCountriesByRegionUseCase(storage).Execute(nameof(AppRegion.NorthAmerica));

        Assert.Empty(new GetSelectedCountriesByRegionUseCase(storage).Execute(nameof(AppRegion.NorthAmerica)));
    }
}

internal sealed class InMemoryStorage : ISettingsStorageService
{
    private readonly Dictionary<AppRegion, IReadOnlyList<string>> _countrySelections = new();
    public UserSession? GetUserSession() => null;
    public void SaveUserSession(UserSession session) { }
    public AppRegion? GetSelectedRegion() => null;
    public void SaveSelectedRegion(AppRegion region) { }
    public IReadOnlyList<string> GetSelectedCountryNames(AppRegion region)
        => _countrySelections.TryGetValue(region, out var names) ? names : Array.Empty<string>();
    public void SaveSelectedCountryNames(AppRegion region, IEnumerable<string> names)
        => _countrySelections[region] = names.ToArray();
}
