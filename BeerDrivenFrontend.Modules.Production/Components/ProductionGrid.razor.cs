using BeerDrivenFrontend.Modules.Production.Events;
using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;

namespace BeerDrivenFrontend.Modules.Production.Components;

public class ProductionGridBase : ComponentBase, IAsyncDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = default!;

    [Parameter]
    public IEnumerable<ProductionOrderJson> ProductionOrders { get; set; } = Enumerable.Empty<ProductionOrderJson>();

    private int _selectedRowNumber = -1;
    protected MudTable<ProductionOrderJson> MudTable = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected Task RowClickEvent(TableRowClickEventArgs<ProductionOrderJson> tableRowClickEventArgs)
    {
        return Bus.Publish(new BrewUpEvent("OrderSelected", JsonSerializer.Serialize(tableRowClickEventArgs.Item)));
    }

    protected string SelectedRowClassFunc(ProductionOrderJson element, int rowNumber)
    {
        if (_selectedRowNumber == rowNumber)
        {
            _selectedRowNumber = -1;
            return string.Empty;
        }

        if (MudTable.SelectedItem == null || !MudTable.SelectedItem.Equals(element))
            return string.Empty;

        _selectedRowNumber = rowNumber;

        return "selected";
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