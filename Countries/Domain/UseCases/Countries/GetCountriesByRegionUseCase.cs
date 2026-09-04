using Core.Services.Storage.Settings.Models;
using Countries.Data;

namespace Countries.Domain.UseCases.Countries;

public sealed class GetCountriesByRegionUseCase(ICountryRepository countryRepository)
{
    public Task<IReadOnlyList<Country>> ExecuteAsync(AppRegion region, CancellationToken cancellationToken = default)
        => countryRepository.GetCountriesByRegionAsync(region, cancellationToken);
}
