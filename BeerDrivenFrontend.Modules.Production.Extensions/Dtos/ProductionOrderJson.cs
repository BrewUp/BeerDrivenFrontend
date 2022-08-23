namespace BeerDrivenFrontend.Modules.Production.Extensions.Dtos;

public class ProductionOrderJson
{
    public string BatchId { get; set; } = string.Empty;
    public string BatchNumber { get; set; } = string.Empty;

    public string BeerId { get; set; } = string.Empty;
    public string BeerType { get; set; } = string.Empty;

    public double QuantityToProduce { get; set; } = 0;
    public double QuantityProduced { get; set; } = 0;

    public DateTime ProductionStartTime { get; set; } = DateTime.MinValue;
    public DateTime ProductionCompleteTime { get; set; } = DateTime.MinValue;

    public string Status { get; set; } = string.Empty;
}