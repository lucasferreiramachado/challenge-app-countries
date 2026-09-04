using Core.Services.Storage.Settings;
using Core.Services.Storage.Settings.Models;

namespace Countries.Domain.UseCases.Countries;

public sealed class SaveSelectedCountriesByRegionUseCase(ISettingsStorageService settingsStorageService)
{
    public void Execute(string regionKey, IEnumerable<string> countryNames)
    {
        settingsStorageService.SaveSelectedCountryNames(Enum.Parse<AppRegion>(regionKey), countryNames);
    }
}

public sealed class GetSelectedCountriesByRegionUseCase(ISettingsStorageService settingsStorageService)
{
    public IReadOnlyList<string> Execute(string regionKey)
    {
        return settingsStorageService.GetSelectedCountryNames(Enum.Parse<AppRegion>(regionKey));
    }
}

public sealed class RemoveSelectedCountryByRegionUseCase(ISettingsStorageService settingsStorageService)
{
    public void Execute(string regionKey, string countryName)
    {
        var region = Enum.Parse<AppRegion>(regionKey);
        var current = settingsStorageService.GetSelectedCountryNames(region).ToList();
        var updated = current.Where(name => !string.Equals(name, countryName, StringComparison.OrdinalIgnoreCase)).ToList();
        settingsStorageService.SaveSelectedCountryNames(region, updated);
    }
}

public sealed class SaveEmptySelectedCountriesByRegionUseCase(ISettingsStorageService settingsStorageService)
{
    public void Execute(string regionKey)
    {
        settingsStorageService.SaveSelectedCountryNames(Enum.Parse<AppRegion>(regionKey), Array.Empty<string>());
    }
}
