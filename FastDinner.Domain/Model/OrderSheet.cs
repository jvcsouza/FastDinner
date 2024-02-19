namespace FastDinner.Domain.Model;

public sealed class OrderSheet
{
    private OrderSheet() { }

    internal OrderSheet(Product product, decimal qty, decimal unitValue)
    {
        Id = Guid.NewGuid();
        Product = product;
        ProductName = product.Name;
        Qty = qty;
        UnitValue = unitValue;
    }

    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; }
    public decimal Qty { get; }
    public decimal Amount => Math.Round(Qty * UnitValue, 2, MidpointRounding.AwayFromZero);
    public decimal UnitValue { get; }

    public Order Order { get; set; }
    public Product Product { get; set; }
}