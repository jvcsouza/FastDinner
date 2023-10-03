namespace FastDinner.Domain.Model;

public class OrderSheet
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }

    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
}