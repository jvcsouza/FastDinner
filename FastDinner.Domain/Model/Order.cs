using FastDinner.Domain.Contracts;

namespace FastDinner.Domain.Model;

public class Order : Entity, IRestaurant
{
    public Guid? CustomerId { get; set; }
    public Guid RestaurantId { get; set; }
    public Guid? TableId { get; set; }
    public decimal Price { get; set; }
    public DateTime OrderDate { get; set; }


    public virtual Table Table { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Restaurant Restaurant { get; set; }
    public ICollection<OrderSheet> Sheets { get; set; }

    public OrderSheet IncludeSheet(OrderSheet sheet)
    {
        Sheets.Add(sheet);

        return sheet;
    }
}