using FastDinner.Domain.Contracts;

namespace FastDinner.Domain.Model;

public sealed class Order : Entity, IRestaurant
{
    private Order() { }

    public Order(Customer customer, Table table)
    {
        Customer = customer;
        Table = table;
    }

    public Guid? CustomerId { get; set; }
    public Guid RestaurantId { get; set; }
    public Guid? TableId { get; set; }
    public decimal Price { get; set; }
    public DateTime OrderDate { get; set; }


    public Table Table { get; set; }
    public Customer Customer { get; set; }
    public Restaurant Restaurant { get; set; }
    public ICollection<OrderSheet> Sheets { get; set; }

    public OrderSheet IncludeSheet(Product product, decimal qty, decimal unitValue)
    {
        var orderSheet = new OrderSheet(product, qty, unitValue);

        Sheets.Add(orderSheet);

        Price += orderSheet.Amount;

        return orderSheet;
    }
}