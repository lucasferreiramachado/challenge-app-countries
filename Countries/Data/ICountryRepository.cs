using Core.Services.Storage.Settings.Models;

namespace Countries.Data;

public interface ICountryRepository
{
    Task<IReadOnlyList<Country>> GetCountriesByRegionAsync(AppRegion region, CancellationToken cancellationToken = default);
}
