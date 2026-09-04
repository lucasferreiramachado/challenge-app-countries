using Core.Services.Http;
using Countries.Data;
using Countries.Domain.UseCases.Countries;
using Countries.Presentation;
using Countries.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Countries;

public static class CountriesExtensions
{
    public static IServiceCollection AddCountriesModule(this IServiceCollection services)
    {
        services.AddTransient<IHttpClient, HttpClientService>();
        services.AddTransient<CountryService>();
        services.AddTransient<ICountryRepository, CountryRepository>();
        services.AddTransient<GetCountriesByRegionUseCase>();
        services.AddTransient<FindCountriesByRegionViewModel>();
        services.AddTransient<FindCountriesByRegionView>();
        services.AddTransient<CountriesListViewModel>();
        services.AddTransient<CountriesListView>();
        return services;
    }
}
