﻿using BeerDrivenFrontend.Modules.Production.Events;
using BlazorComponentBus;
using Microsoft.AspNetCore.Components;

namespace BeerDrivenFrontend.Modules.Production;

public class ProductionBase : ComponentBase, IDisposable
{
    [Inject] private ComponentBus Bus { get; set; } = default!;

    protected string Message { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Bus.Subscribe<OrderBeerEvent>(MessageAddedHandler);
        await base.OnInitializedAsync();
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