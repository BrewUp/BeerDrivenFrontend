using BeerDrivenFrontend.Modules.Production.Events;
using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace BeerDrivenFrontend.Modules.Production.Components;

public class BeerProductionOrderBase : ComponentBase, IAsyncDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = default!;
    [Parameter] public IEnumerable<BeerLookupJson> Beers { get; set; } = Enumerable.Empty<BeerLookupJson>();
    [Parameter] public OrderJson Order { get; set; } = new();

    protected BeerLookupJson CurrentBeer { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!string.IsNullOrEmpty(Order.BeerType))
            CurrentBeer = Beers.FirstOrDefault(b => b.BeerId.Equals(Order.BeerId, StringComparison.Ordinal));
    }

    protected Task OnOrderBeer()
    {
        if (string.IsNullOrEmpty(CurrentBeer.BeerType))
            return Task.CompletedTask;

        Order.BeerId = CurrentBeer.BeerId;
        Order.BeerType = CurrentBeer.BeerType;

        // TODO: FluentValidation
        return Bus.Publish(new BrewUpEvent("SendOrderBeer", JsonSerializer.Serialize(Order)));
    }

    #region Dispose
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncInternal();
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncInternal()
    {
        // Async cleanup mock
        await Task.Yield();
    }
    #endregion
}