using Core.Services.Storage.Settings.Models;
using Countries.Services;

namespace Countries.Data;

public sealed class CountryRepository(CountryService countryService) : ICountryRepository
{
    public Task<IReadOnlyList<Country>> GetCountriesByRegionAsync(AppRegion region, CancellationToken cancellationToken = default)
        => countryService.GetCountriesByRegionAsync(region, cancellationToken);
}
