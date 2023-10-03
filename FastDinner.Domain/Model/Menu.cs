using FastDinner.Domain.Contracts;

namespace FastDinner.Domain.Model;

public class Menu : Entity, IRestaurant
{
    public Menu(string name, string description, string image)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Image = image;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public Guid RestaurantId { get; set; }
    public bool Main { get; set; }

    public virtual Restaurant Restaurant { get; set; }
    public virtual ICollection<MenuCategory> Categories { get; set; }

    public void Update(string name, string description, string image)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name is required");

        Name = name;
        Description = description;
        Image = image ?? Image;
    }

    public void AddCategory(MenuCategory category)
    {
        category.MenuId = Id;
        Categories.Add(category);
    }
}