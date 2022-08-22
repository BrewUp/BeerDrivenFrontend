using BeerDrivenFrontend.Modules.Production.Events;
using BeerDrivenFrontend.Modules.Production.Extensions.Abstracts;
using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;
using BlazorComponentBus;
using Microsoft.AspNetCore.Components;

namespace BeerDrivenFrontend.Modules.Production;

public class ProductionBase : ComponentBase, IDisposable
{
    [Inject] private ComponentBus Bus { get; set; } = default!;
    [Inject] private IProductionService ProductionService { get; set; } = default!;

    protected string Message { get; set; } = string.Empty;
    protected IEnumerable<BeerJson> Beers { get; set; } = Enumerable.Empty<BeerJson>();

    protected override async Task OnInitializedAsync()
    {
        Bus.Subscribe<OrderBeerEvent>(MessageAddedHandler);

        await LoadBeersAsync();

        await base.OnInitializedAsync();
    }

    private async Task LoadBeersAsync()
    {
        Beers = await ProductionService.GetBeersAsync();
    }

    private void MessageAddedHandler(MessageArgs args)
    {
        Message = args.GetMessage<OrderBeerEvent>().Message;

        StateHasChanged();
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