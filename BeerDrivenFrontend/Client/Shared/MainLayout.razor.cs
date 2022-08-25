using Microsoft.AspNetCore.Components;
using MudBlazor.ThemeManager;

namespace BeerDrivenFrontend.Client.Shared;

public class MainLayoutBase : LayoutComponentBase, IDisposable
{
    protected bool DrawerOpen = true;
    protected ThemeManagerTheme ThemeManager = new();
    public bool ThemeManagerOpen;

    public async ValueTask DisposeAsync()
    {
    }

    protected override void OnInitialized()
    {
        StateHasChanged();
    }

    protected void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }

    protected void OpenThemeManager(bool value)
    {
        ThemeManagerOpen = value;
    }

    protected void UpdateTheme(ThemeManagerTheme value)
    {
        ThemeManager = value;
        StateHasChanged();
    }

    #region Dispose
    private void Dispose(bool disposing)
    {   
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~MainLayoutBase()
    {
        Dispose(false);
    }
    #endregion
}