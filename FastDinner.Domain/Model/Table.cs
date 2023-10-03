using FastDinner.Domain.Contracts;

namespace FastDinner.Domain.Model;

public class Table : Entity, IRestaurant
{
    public Table(string description, int seats)
    {
        Id = Guid.NewGuid();

        Description = description;
        Seats = seats;
    }

    public string Description { get; set; }
    public int Seats { get; set; }
    public Guid RestaurantId { get; set; }

    public virtual Restaurant Restaurant { get; set; }
}