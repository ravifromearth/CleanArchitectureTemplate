using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Domain.Enums;
using Bogus;

namespace EntityFrameworkCore8Samples.Tests.Builders;

public class OrderBuilder
{
    private readonly Faker<Order> _faker;
    private Order _order;

    public OrderBuilder()
    {
        _faker = new Faker<Order>()
            .RuleFor(o => o.Id, f => f.Random.Guid())
            .RuleFor(o => o.UserId, f => f.Random.Guid())
            .RuleFor(o => o.OrderNumber, f => f.Random.AlphaNumeric(10).ToUpper())
            .RuleFor(o => o.ShippedAt, f => f.Random.Bool(0.7f) ? f.Date.Recent(30) : null)
            .RuleFor(o => o.DeliveredAt, f => f.Random.Bool(0.5f) ? f.Date.Recent(15) : null)
            .RuleFor(o => o.SubTotal, f => f.Random.Decimal(50, 2000))
            .RuleFor(o => o.TaxAmount, f => f.Random.Decimal(5, 200))
            .RuleFor(o => o.ShippingCost, f => f.Random.Decimal(0, 50))
            .RuleFor(o => o.Total, f => f.Random.Decimal(55, 2250))
            .RuleFor(o => o.OrderData, f => f.Random.Bool(0.3f) ? f.Random.String(100) : null)
            .RuleFor(o => o.Tags, f => f.Random.Bool(0.4f) ? f.Random.StringArray(3, 5, 10) : null)
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>())
            .RuleFor(o => o.PaymentMethod, f => f.PickRandom<PaymentMethod>())
            .RuleFor(o => o.ShippingAddress, f => f.Address.FullAddress())
            .RuleFor(o => o.BillingAddress, f => f.Address.FullAddress())
            .RuleFor(o => o.ReceiptPdf, f => f.Random.Bool(0.2f) ? f.Random.Bytes(1024) : null)
            .RuleFor(o => o.CreatedAt, f => f.Date.Past(1))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Recent());

        _order = _faker.Generate();
    }

    public OrderBuilder WithId(Guid id)
    {
        _order.Id = id;
        return this;
    }

    public OrderBuilder WithUserId(Guid userId)
    {
        _order.UserId = userId;
        return this;
    }

    public OrderBuilder WithOrderNumber(string orderNumber)
    {
        _order.OrderNumber = orderNumber;
        return this;
    }

    public OrderBuilder WithStatus(OrderStatus status)
    {
        _order.Status = status;
        return this;
    }

    public OrderBuilder WithPaymentMethod(PaymentMethod paymentMethod)
    {
        _order.PaymentMethod = paymentMethod;
        return this;
    }

    public OrderBuilder WithSubTotal(decimal subTotal)
    {
        _order.SubTotal = subTotal;
        return this;
    }

    public OrderBuilder WithTaxAmount(decimal taxAmount)
    {
        _order.TaxAmount = taxAmount;
        return this;
    }

    public OrderBuilder WithShippingCost(decimal shippingCost)
    {
        _order.ShippingCost = shippingCost;
        return this;
    }

    public OrderBuilder WithTotal(decimal total)
    {
        _order.Total = total;
        return this;
    }

    public OrderBuilder WithShippingAddress(string shippingAddress)
    {
        _order.ShippingAddress = shippingAddress;
        return this;
    }

    public OrderBuilder WithBillingAddress(string billingAddress)
    {
        _order.BillingAddress = billingAddress;
        return this;
    }

    public OrderBuilder WithShippedAt(DateTime? shippedAt)
    {
        _order.ShippedAt = shippedAt;
        return this;
    }

    public OrderBuilder WithDeliveredAt(DateTime? deliveredAt)
    {
        _order.DeliveredAt = deliveredAt;
        return this;
    }

    public OrderBuilder WithOrderData(string? orderData)
    {
        _order.OrderData = orderData;
        return this;
    }

    public OrderBuilder WithTags(string[]? tags)
    {
        _order.Tags = tags;
        return this;
    }

    public OrderBuilder WithReceiptPdf(byte[]? receiptPdf)
    {
        _order.ReceiptPdf = receiptPdf;
        return this;
    }

    public Order Build()
    {
        return _order;
    }

    public static OrderBuilder Create() => new();
    public static OrderBuilder CreateRandom() => new();
}


