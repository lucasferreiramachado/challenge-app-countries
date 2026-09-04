using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Services.Storage.Settings;
using Core.Services.Storage.Settings.Models;

namespace Countries.Presentation;

public partial class FindCountriesByRegionViewModel : ObservableObject
{
    private readonly ISettingsStorageService _settingsStorageService;

    [ObservableProperty]
    private ObservableCollection<SelectedCountryMoment> _northAmericaSelectedCountries = new();

    [ObservableProperty]
    private ObservableCollection<SelectedCountryMoment> _southAmericaSelectedCountries = new();

    public FindCountriesByRegionViewModel(ISettingsStorageService settingsStorageService)
    {
        _settingsStorageService = settingsStorageService;
        LoadSelections();
    }

    public void LoadSelections()
    {
        NorthAmericaSelectedCountries = new ObservableCollection<SelectedCountryMoment>(
            _settingsStorageService.GetSelectedCountryNames(AppRegion.NorthAmerica)
                .Select(name => new SelectedCountryMoment(AppRegion.NorthAmerica, name)));

        SouthAmericaSelectedCountries = new ObservableCollection<SelectedCountryMoment>(
            _settingsStorageService.GetSelectedCountryNames(AppRegion.SouthAmerica)
                .Select(name => new SelectedCountryMoment(AppRegion.SouthAmerica, name)));
    }

    [RelayCommand]
    private async Task VoltarAsync()
    {
        await Shell.Current.GoToAsync("//Home");
    }

    [RelayCommand]
    private async Task OpenNorthAmericaAsync()
    {
        await OpenRegionAsync(AppRegion.NorthAmerica);
    }

    [RelayCommand]
    private async Task OpenSouthAmericaAsync()
    {
        await OpenRegionAsync(AppRegion.SouthAmerica);
    }

    [RelayCommand]
    private void RemoveCountry(SelectedCountryMoment selectedCountry)
    {
        if (selectedCountry is null || string.IsNullOrWhiteSpace(selectedCountry.CountryName))
            return;

        var current = _settingsStorageService.GetSelectedCountryNames(selectedCountry.Region).ToList();
        var updated = current.Where(name => !string.Equals(name, selectedCountry.CountryName, StringComparison.OrdinalIgnoreCase)).ToList();
        _settingsStorageService.SaveSelectedCountryNames(selectedCountry.Region, updated);
        LoadSelections();
    }

    private Task OpenRegionAsync(AppRegion region)
    {
        _settingsStorageService.SaveSelectedRegion(region);

        var route = region == AppRegion.NorthAmerica
            ? "//CountriesList?region=NorthAmerica"
            : "//CountriesList?region=SouthAmerica";

        return Shell.Current.GoToAsync(route);
    }
}

public sealed record SelectedCountryMoment(AppRegion Region, string CountryName);
