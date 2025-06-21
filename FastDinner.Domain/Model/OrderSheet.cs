using System.ComponentModel.DataAnnotations;

namespace FastDinner.Domain.Model;

public sealed class OrderSheet
{
    private OrderSheet() { }

    internal OrderSheet(Product product, decimal qty, decimal unitValue)
    {
        Id = Guid.NewGuid();
        Product = product;
        ProductName = product.Name;
        UnitValue = unitValue;
        Qty = qty;
        PrepareStatus = OrderPrepareStatus.Waiting;
    }

    internal OrderSheet(Guid groupedSheet, Product product, decimal qty, decimal unitValue)
        : this(product, qty, unitValue)
    {
        GroupedSheet = groupedSheet;
    }

    [Key]
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; }
    public decimal Qty { get; }
    public decimal Amount => Math.Round(Qty * UnitValue, 2, MidpointRounding.AwayFromZero);
    public decimal UnitValue { get; }
    public Guid? GroupedSheet { get; set; }
    public OrderPrepareStatus PrepareStatus { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
}