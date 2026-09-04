using Core.Services.Storage.Settings;
using DesignSystem;
using Countries;

namespace Example;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .UseDesignSystem();

        builder.Services.AddSingleton<ISettingsStorageService, ExampleSettingsStorage>();
        builder.Services.AddCountriesModule();
        return builder.Build();
    }
}

internal sealed class ExampleSettingsStorage : ISettingsStorageService
{
    public Core.Services.Storage.Settings.Models.UserSession? GetUserSession() => null;
    public void SaveUserSession(Core.Services.Storage.Settings.Models.UserSession session) { }
    public Core.Services.Storage.Settings.Models.AppRegion? GetSelectedRegion() => null;
    public void SaveSelectedRegion(Core.Services.Storage.Settings.Models.AppRegion region) { }
    public IReadOnlyList<string> GetSelectedCountryNames(Core.Services.Storage.Settings.Models.AppRegion region) => [];
    public void SaveSelectedCountryNames(Core.Services.Storage.Settings.Models.AppRegion region, IEnumerable<string> names) { }
}
