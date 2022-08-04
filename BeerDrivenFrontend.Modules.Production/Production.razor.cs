using BeerDrivenFrontend.Shared.Events;
using Microsoft.AspNetCore.Components;

namespace BeerDrivenFrontend.Modules.Production;

public class ProductionBase : ComponentBase, IDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected Task OnLoadBeers()
    {
        return Bus.Publish(new SayHelloBrewer("Hello From Production"));
    }

    #region Dispose
    private static void Dispose(bool disposing)
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

    ~ProductionBase()
    {
        Dispose(false);
    }
    #endregion
}