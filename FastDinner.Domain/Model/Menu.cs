using FastDinner.Domain.Contracts;

namespace FastDinner.Domain.Model;

public class Menu : Entity, IRestaurant
{
    internal Menu(string name, string description, string image, bool main)
    {
        //Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Image = image;
        Main = main;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public Guid RestaurantId { get; set; }
    public bool Main { get; set; }
    public byte[] Ver { get; }

    public virtual Restaurant Restaurant { get; set; }
    public virtual ICollection<MenuCategory> Categories { get; set; }

    public void SetUpToMain() => Main = true;
    public void SetUpToSecondary() => Main = false;

    public void Update(string name, string description, string image)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name is required");

        Name = name;
        Description = description;
        Image = image ?? Image;
    }

    public MenuCategory AddCategory(string category, string description)
    {
        var newCategory = new MenuCategory(category, description);

        Categories.Add(newCategory);

        return newCategory;
    }

    public MenuCategory AddCategory(MenuCategory menuCategory)
    {
        Categories ??= new List<MenuCategory>();

        Categories.Add(menuCategory);

        return menuCategory;
    }

    public void AddProduct(Product product, MenuCategory category, string description, decimal price)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product));

        if (category is null)
            throw new ArgumentNullException(nameof(category));

        var categoryToInclude = Categories.FirstOrDefault(x => x.Id == category.Id);

        if (categoryToInclude is null)
            throw new ApplicationException($"Category with id {category.Id} not found");

        var menuItem = new MenuItem(product.Id, product.Name, description, price);

        categoryToInclude.AddItem(menuItem);
    }
}