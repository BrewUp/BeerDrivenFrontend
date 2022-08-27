using BeerDrivenFrontend.Modules.Pubs.Events;
using BeerDrivenFrontend.Modules.Pubs.Extensions.Abstracts;
using BeerDrivenFrontend.Modules.Pubs.Extensions.Dtos;
using BlazorComponentBus;
using Microsoft.AspNetCore.Components;

namespace BeerDrivenFrontend.Modules.Pubs;

public class PubsBase : ComponentBase, IDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = default!;
    [Inject] private IBeerService BeerService { get; set; } = default!;

    protected IEnumerable<BeerJson> Beers { get; set; } = Enumerable.Empty<BeerJson>();

    protected string Message { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await OnSayHelloBrewers();

        Bus.Subscribe<SayHelloBrewer>(MessageAddedHandler);

        await LoadBeersAsync();

        await base.OnInitializedAsync();
    }

    private async Task LoadBeersAsync()
    {
        Beers = await BeerService.GetBeersAsync();
    }

    private void MessageAddedHandler(MessageArgs args)
    {
        Message = args.GetMessage<SayHelloBrewer>().Message;
        StateHasChanged();
    }

    protected Task OnSayHelloBrewers()
    {
        return Task.CompletedTask;
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
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~PubsBase()
    {
        Dispose(false);
    }
    #endregion
}