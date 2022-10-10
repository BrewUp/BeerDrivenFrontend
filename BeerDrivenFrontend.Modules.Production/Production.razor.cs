using BeerDrivenFrontend.Modules.Production.Events;
using BeerDrivenFrontend.Modules.Production.Extensions.Abstracts;
using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;
using BeerDrivenFrontend.Shared.Configuration;
using BlazorComponentBus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace BeerDrivenFrontend.Modules.Production;

public class ProductionBase : ComponentBase, IAsyncDisposable
{
    [Inject] private ComponentBus Bus { get; set; } = default!;
    [Inject] private IProductionService ProductionService { get; set; } = default!;
    [Inject] private AppConfiguration Configuration { get; set; } = new();

    protected string Message { get; set; } = string.Empty;
    protected IEnumerable<ProductionOrderJson> ProductionOrders { get; set; } = Enumerable.Empty<ProductionOrderJson>();
    protected IEnumerable<BeerLookupJson> Beers { get; set; } = Enumerable.Empty<BeerLookupJson>();

    protected OrderJson CurrentOrder = new();
    protected ProductionOrderJson CurrentProductionOrder { get; set; } = new();
    protected bool ShowOrder = false;

    private HubConnection? _hubConnection;

    protected override async Task OnInitializedAsync()
    {
        Bus.Subscribe<BrewUpEvent>(MessageAddedHandler);

        await LoadProductionOrderAsync();
        await LoadBeersAsync();

        await Connect();

        await base.OnInitializedAsync();
    }

    private async Task Connect()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{Configuration.SignalRUri}hubs/production")
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string>("beerProductionStarted", async (message) =>
        {
            await LoadProductionOrderAsync();
            await Bus.Publish(new BrewUpEvent($"An update was received: {DateTime.Now}", string.Empty));
            StateHasChanged();
        });

        _hubConnection.Closed += exception =>
        {
            if (exception != null)
                Bus.Publish(new BrewUpEvent(exception.Message, string.Empty));

            return null;
        };

        try
        {
            await _hubConnection.StartAsync();
            await Bus.Publish(new BrewUpEvent(
                $"signalR Connection successfully established. ConnectionId: {_hubConnection.ConnectionId} - Uri: {Configuration.ProductionApiUri}/hubs/production", string.Empty));
        }
        catch (Exception ex)
        {
            await Bus.Publish(new BrewUpEvent($"{ex.Message}", string.Empty));
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

    private void GetOrderSelected(BrewUpEvent brewUpEvent)
    {
        if (brewUpEvent is null || string.IsNullOrEmpty(brewUpEvent.Body))
            return;

        CurrentProductionOrder = JsonSerializer.Deserialize<ProductionOrderJson>(brewUpEvent.Body);
        Message = $"Order Selected {CurrentProductionOrder!.BatchNumber}";

        StateHasChanged();
    }

    private void PrepareNewOrder()
    {
        CurrentOrder.BeerId = CurrentProductionOrder.BeerId;
        CurrentOrder.BeerType = CurrentProductionOrder.BeerType;
        CurrentProductionOrder.BatchNumber = $"{DateTime.Now.Year:0000}{DateTime.Now.Month:00}{DateTime.Now.Day:00}-";
        CurrentOrder.ProductionTime = DateTime.Now;

        ShowOrder = true;
    }

    private void PrepareCompleteOrder()
    {
        if (CurrentProductionOrder.QuantityProduced > 0)
        {
            Message = $"Order {CurrentProductionOrder.BatchNumber} already closed!";
            StateHasChanged();
            return;
        }

        CurrentOrder.BeerId = CurrentProductionOrder.BeerId;
        CurrentOrder.BeerType = CurrentProductionOrder.BeerType;
        CurrentOrder.Quantity = CurrentProductionOrder.QuantityToProduce;
        CurrentOrder.BatchId = CurrentProductionOrder.BatchId;
        CurrentOrder.BatchNumber = CurrentProductionOrder.BatchNumber;
        CurrentProductionOrder.BatchNumber = $"{DateTime.Now.Year:0000}{DateTime.Now.Month:00}{DateTime.Now.Day:00}-";
        CurrentOrder.ProductionTime = DateTime.Now;

        ShowOrder = true;
    }

    private async Task SendProductionOrderAsync(BrewUpEvent brewUpEvent)
    {
        if (brewUpEvent is null || string.IsNullOrEmpty(brewUpEvent.Body))
            return;

        var order = JsonSerializer.Deserialize<OrderJson>(brewUpEvent.Body);
        if (order is null)
            return;

        order.ProductionTime = order.ProductionTime.ToUniversalTime();

        var chkOrder = ProductionOrders.FirstOrDefault(o => o.BatchNumber.Equals(order.BatchNumber));
        if (chkOrder == null)
        {
            order.BeerId = Guid.NewGuid().ToString();
            await ProductionService.SendStartProductionOrderAsync(order);
        }
        else
            await ProductionService.SendCompleteProductionOrderAsync(order);

        ShowOrder = false;
        StateHasChanged();
    }

    private void MessageAddedHandler(MessageArgs args)
    {
        Message = args.GetMessage<BrewUpEvent>().Message;

        switch (Message)
        {
            case "OrderSelected":
                GetOrderSelected(args.GetMessage<BrewUpEvent>());
                break;

            case "AddOrderBeer":
                PrepareNewOrder();
                break;

            case "CompleteOrderBeer":
                PrepareCompleteOrder();
                break;

            case "SendOrderBeer":
                Task.Run(async() => await SendProductionOrderAsync(args.GetMessage<BrewUpEvent>()));
                break;
        }

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