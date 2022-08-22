using BeerDrivenFrontend.Modules.Production.Events;
using BeerDrivenFrontend.Modules.Production.Extensions.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BeerDrivenFrontend.Modules.Production.Components;

public class ProductionGridBase : ComponentBase, IDisposable
{
    [Inject] private BlazorComponentBus.ComponentBus Bus { get; set; } = default!;
    [Parameter] public IEnumerable<BeerJson> Beers { get; set; } = Enumerable.Empty<BeerJson>();

    private int _selectedRowNumber = -1;
    protected MudTable<BeerJson> MudTable = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    protected Task RowClickEvent(TableRowClickEventArgs<BeerJson> tableRowClickEventArgs)
    {
        return Bus.Publish(new OrderBeerEvent($"Beer selected {tableRowClickEventArgs.Item.BeerId}"));
    }

    protected string SelectedRowClassFunc(BeerJson element, int rowNumber)
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

    ~ProductionGridBase()
    {
        Dispose(false);
    }
    #endregion
}