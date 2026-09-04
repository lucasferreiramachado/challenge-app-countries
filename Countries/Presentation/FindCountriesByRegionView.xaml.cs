namespace Countries.Presentation;

public partial class FindCountriesByRegionView : ContentPage
{
    private readonly FindCountriesByRegionViewModel _viewModel;

    public FindCountriesByRegionView(FindCountriesByRegionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadSelections();
    }
}
