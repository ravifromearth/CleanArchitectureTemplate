using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Domain.Enums;
using Bogus;

namespace EntityFrameworkCore8Samples.Tests.Builders;

public class ProductBuilder
{
    private readonly Faker<Product> _faker;
    private Product _product;

    public ProductBuilder()
    {
        _faker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Sku, f => f.Commerce.Ean13())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
            .RuleFor(p => p.SalePrice, f => f.Random.Bool(0.3f) ? f.Random.Decimal(5, 800) : null)
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
            .RuleFor(p => p.Brand, f => f.Commerce.Department())
            .RuleFor(p => p.Weight, f => f.Random.Decimal(0.1m, 50))
            .RuleFor(p => p.Dimensions, f => $"{f.Random.Decimal(10, 100)}x{f.Random.Decimal(10, 100)}x{f.Random.Decimal(5, 50)}")
            .RuleFor(p => p.Color, f => f.Commerce.Color())
            .RuleFor(p => p.Material, f => f.Commerce.ProductMaterial())
            .RuleFor(p => p.Status, f => f.PickRandom<ProductStatus>())
            .RuleFor(p => p.Type, f => f.PickRandom<ProductType>())
            .RuleFor(p => p.CreatedAt, f => f.Date.Past(2))
            .RuleFor(p => p.UpdatedAt, f => f.Date.Recent())
            .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f));

        _product = _faker.Generate();
    }

    public ProductBuilder WithId(Guid id)
    {
        _product.Id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _product.Name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _product.Description = description;
        return this;
    }

    public ProductBuilder WithSku(string sku)
    {
        _product.Sku = sku;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _product.Price = price;
        return this;
    }

    public ProductBuilder WithSalePrice(decimal? salePrice)
    {
        _product.SalePrice = salePrice;
        return this;
    }

    public ProductBuilder WithCategory(string category)
    {
        _product.Category = category;
        return this;
    }

    public ProductBuilder WithBrand(string brand)
    {
        _product.Brand = brand;
        return this;
    }

    public ProductBuilder WithStatus(ProductStatus status)
    {
        _product.Status = status;
        return this;
    }

    public ProductBuilder WithType(ProductType type)
    {
        _product.Type = type;
        return this;
    }

    public ProductBuilder AsActive()
    {
        _product.IsActive = true;
        return this;
    }

    public ProductBuilder AsInactive()
    {
        _product.IsActive = false;
        return this;
    }

    public ProductBuilder WithWeight(decimal weight)
    {
        _product.Weight = weight;
        return this;
    }

    public ProductBuilder WithDimensions(string dimensions)
    {
        _product.Dimensions = dimensions;
        return this;
    }

    public ProductBuilder WithColor(string color)
    {
        _product.Color = color;
        return this;
    }

    public ProductBuilder WithMaterial(string material)
    {
        _product.Material = material;
        return this;
    }

    public Product Build()
    {
        return _product;
    }

    public static ProductBuilder Create() => new();
    public static ProductBuilder CreateRandom() => new();
}



