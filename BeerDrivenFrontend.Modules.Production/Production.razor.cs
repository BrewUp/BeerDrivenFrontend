using BeerDrivenFrontend.Modules.Production.Events;
using BeerDrivenFrontend.Modules.Production.Extensions.Abstracts;
using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;
using BeerDrivenFrontend.Shared.Configuration;
using BlazorComponentBus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BeerDrivenFrontend.Modules.Production;

public class ProductionBase : ComponentBase, IAsyncDisposable
{
    [Inject] private ComponentBus Bus { get; set; } = default!;
    [Inject] private IProductionService ProductionService { get; set; } = default!;
    [Inject] private AppConfiguration Configuration { get; set; } = new();

    protected string Message { get; set; } = string.Empty;
    protected IEnumerable<BeerJson> Beers { get; set; } = Enumerable.Empty<BeerJson>();
    protected IEnumerable<ProductionOrderJson> ProductionOrders { get; set; } = Enumerable.Empty<ProductionOrderJson>();

    private HubConnection? _hubConnection = default!;

    protected override async Task OnInitializedAsync()
    {
        Bus.Subscribe<OrderBeerEvent>(MessageAddedHandler);

        await LoadBeersAsync();
        await LoadProductionOrderAsync();

        await Connect();

        await base.OnInitializedAsync();
    }

    private async Task Connect()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{Configuration.ProductionApiUri}hubs/production")
            .Build();

        _hubConnection.On<string>("BeerProductionStarted", async (message) =>
        {
            await LoadProductionOrderAsync();
            Console.WriteLine($"Message Received: {message}");
            StateHasChanged();
        });

        try
        {
            await _hubConnection.StartAsync();
            await Bus.Publish(new OrderBeerEvent("signalR Connection successfully established"));
        }
        catch (Exception e)
        {
            await Bus.Publish(new OrderBeerEvent($"{e.Message}"));
            Console.WriteLine(e);
            Console.WriteLine($"{Configuration.ProductionApiUri}/hubs/production");
            throw;
        }
    }

    private async Task LoadProductionOrderAsync()
    {
        ProductionOrders = await ProductionService.GetProductionOrdersAsync();
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
    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

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