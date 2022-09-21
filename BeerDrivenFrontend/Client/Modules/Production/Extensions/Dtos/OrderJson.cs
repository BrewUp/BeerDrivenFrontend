namespace BeerDrivenFrontend.Client.Modules.Production.Extensions.Dtos;

public class OrderJson
{
    public string BeerId { get; set; } = string.Empty;
    public string BeerType { get; set; } = string.Empty;

    public string BatchNumber { get; set; } = string.Empty;
    public double Quantity { get; set; } = 0;

    public DateTime ProductionTime { get; set; } = DateTime.MinValue;
}