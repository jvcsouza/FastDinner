namespace FastDinner.Domain.Model;

public class Order
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? TableId { get; set; }
    public decimal Price { get; set; }
    public DateTime OrderDate { get; set; }


    public virtual Table Table { get; set; }
    public virtual Customer Customer { get; set; }
    public ICollection<OrderSheet> Sheets { get; set; }
}