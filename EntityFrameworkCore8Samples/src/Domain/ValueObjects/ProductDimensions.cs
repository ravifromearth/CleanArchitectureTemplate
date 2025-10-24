namespace EntityFrameworkCore8Samples.Domain.ValueObjects;

public class ProductDimensions
{
    public decimal Length { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public string Unit { get; set; } = "cm";
}


