namespace FastDinner.Domain.Model;

public class Menu
{
    public Menu(string name, string description, string image)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Image = image;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public Guid RestaurantId { get; set; }
    public bool Main { get; set; }

    public virtual Restaurant Restaurant { get; set; }
    public ICollection<MenuCategory> MenuCategories { get; set; }

    public void Update(string name, string description, string image)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name is required");

        Name = name;
        Description = description;
        Image = image ?? Image;
    }
}