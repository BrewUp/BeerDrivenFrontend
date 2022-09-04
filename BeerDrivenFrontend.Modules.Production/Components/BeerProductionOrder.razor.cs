﻿using BeerDrivenFrontend.Modules.Production.Events;
using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace BeerDrivenFrontend.Modules.Production.Components;

public class BeerProductionOrderBase : ComponentBase, IAsyncDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = default!;
    [Parameter] public OrderJson Order { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected Task OnOrderBeer()
    {
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