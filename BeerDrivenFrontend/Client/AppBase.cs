using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

namespace BeerDrivenFrontend.Client;

public class AppBase : ComponentBase, IDisposable
{
    [Inject] private LazyAssemblyLoader AssemblyLoader { get; set; } = default!;
    [Inject] private ILogger<App> Logger { get; set; } = default!;

    protected readonly List<Assembly> LazyLoadedAssemblies = new();

    protected async Task OnNavigateAsync(NavigationContext args)
    {

    }

    #region Dispose
    public void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~AppBase()
    {
        Dispose(false);
    }
    #endregion
}