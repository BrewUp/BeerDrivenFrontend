using BeerDrivenFrontend.Modules.Production.Events;
using Microsoft.AspNetCore.Components;

namespace BeerDrivenFrontend.Modules.Production.Components;

public class ProductionToolbarBase : ComponentBase, IDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected Task OnStartOrderBeer()
    {
        return Bus.Publish(new BrewUpEvent("AddOrderBeer", null));
    }

    protected Task OnCompleteOrderBeer()
    {
        return Bus.Publish(new BrewUpEvent("CompleteOrderBeer", null));
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

    ~ProductionToolbarBase()
    {
        Dispose(false);
    }
    #endregion
}