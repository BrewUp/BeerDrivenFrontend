using BeerDrivenFrontend.Modules.Pubs.Events;
using Microsoft.AspNetCore.Components;

namespace BeerDrivenFrontend.Modules.Pubs.Components;

public class PubsToolbarBase : ComponentBase, IDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected Task OnOrderBeer()
    {
        return Bus.Publish(new SayHelloBrewer("Hey ... Hello Brewers"));
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

    ~PubsToolbarBase()
    {
        Dispose(false);
    }
    #endregion
}