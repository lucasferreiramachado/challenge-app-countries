using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Presentation;
using Core.Services.Storage.Settings;
using Core.Services.Storage.Settings.Models;
using Countries.Domain.UseCases.Countries;

namespace Countries.Presentation;

public partial class CountriesListViewModel : ViewModelBase
{
    private readonly GetCountriesByRegionUseCase _getCountriesByRegionUseCase;
    private readonly ISettingsStorageService _settingsStorageService;
    private AppRegion _region;

    [ObservableProperty]
    private ObservableCollection<CountryListItem> _countries = new();

    [ObservableProperty]
    private string _regionTitle = string.Empty;
    [ObservableProperty]
    private string _emptyMessage = string.Empty;
    [ObservableProperty]
    private bool _isSuccess;
    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private bool _hasError;
    
    public CountriesListViewModel(GetCountriesByRegionUseCase getCountriesByRegionUseCase, ISettingsStorageService settingsStorageService)
    {
        _getCountriesByRegionUseCase = getCountriesByRegionUseCase;
        _settingsStorageService = settingsStorageService;
    }

    public async Task SetupView()
    {
        IsSuccess = false;
        HasError = false;
        EmptyMessage = string.Empty;
        ErrorMessage = null;
        IsLoading = true;
        
        _region = ResolveRegionFromShell();
        RegionTitle = _region.DisplayName();

        var selectedNames = _settingsStorageService.GetSelectedCountryNames(_region)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        
        try
        {
            IsBusy = true;
            var countries = await _getCountriesByRegionUseCase.ExecuteAsync(_region);

            Countries = new ObservableCollection<CountryListItem>(
                countries.Select(country => new CountryListItem
                {
                    Name = country.Name,
                    FlagUrl = country.FlagUrl,
                    IsSelected = selectedNames.Contains(country.Name)
                }));

            if (Countries.Count == 0)
                EmptyMessage = "Nenhum item foi retornado.";
            
            IsLoading = false;
            IsSuccess = true;
        }
        catch (Exception ex)
        {
            Countries = new ObservableCollection<CountryListItem>();
            ErrorMessage = "Um erro aconteceu. Não foi possivel obter a lista de países.";
            IsLoading = false;
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task VoltarAsync()
    {
        await Shell.Current.GoToAsync("//Countries");
    }

    [RelayCommand]
    private async Task FinishSelectionAsync()
    {
        var selectedNames = Countries
            .Where(country => country.IsSelected)
            .Select(country => country.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        _settingsStorageService.SaveSelectedCountryNames(_region, selectedNames);
        await Shell.Current.GoToAsync("//Countries");
    }

    private AppRegion ResolveRegionFromShell()
    {
        var location = Shell.Current?.CurrentState?.Location?.ToString() ?? string.Empty;
        var query = location.Contains('?') ? location[(location.IndexOf('?') + 1)..] : string.Empty;
        var value = ExtractQueryValue(query, "region");

        if (Enum.TryParse<AppRegion>(value, ignoreCase: true, out var region))
            return region;

        if (string.Equals(value, "North America", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(value, "América do Norte", StringComparison.OrdinalIgnoreCase))
            return AppRegion.NorthAmerica;

        if (string.Equals(value, "South America", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(value, "América do Sul", StringComparison.OrdinalIgnoreCase))
            return AppRegion.SouthAmerica;

        var selectedRegion = _settingsStorageService.GetSelectedRegion();
        if (selectedRegion is not null)
            return selectedRegion.Value;

        return AppRegion.NorthAmerica;
    }

    private static string ExtractQueryValue(string query, string key)
    {
        if (string.IsNullOrWhiteSpace(query))
            return string.Empty;

        foreach (var segment in query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = segment.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 && string.Equals(parts[0], key, StringComparison.OrdinalIgnoreCase))
                return Uri.UnescapeDataString(parts[1]);
        }

        return string.Empty;
    }
}

public partial class CountryListItem : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _flagUrl = string.Empty;

    [ObservableProperty]
    private bool _isSelected;
}
