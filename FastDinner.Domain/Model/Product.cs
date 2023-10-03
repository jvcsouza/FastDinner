using FastDinner.Domain.Contracts;

namespace FastDinner.Domain.Model;

public class Product : Entity, IRestaurant
{
    public Product(string name)
    {
        Id = Guid.NewGuid();

        Update(name);
    }

    public string Name { get; set; }
    public Guid RestaurantId { get; set; }

    public void Update(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name is required");

        Name = name;
    }
}