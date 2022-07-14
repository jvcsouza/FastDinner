namespace FastDinner.Domain.Model;

public class Table
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int Seats { get; set; }
    public Guid RestaurantId { get; set; }
    
    public virtual Restaurant Restaurant { get; set; }
}