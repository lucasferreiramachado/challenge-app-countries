using Core.Services.Storage.Settings;
using Core.Services.Storage.Settings.Models;

namespace Countries.Presentation;

public partial class CountriesListView : ContentPage
{
    private readonly CountriesListViewModel _viewModel;

    public CountriesListView(CountriesListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.SetupView();
    }
}