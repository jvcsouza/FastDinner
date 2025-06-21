namespace FastDinner.Domain.Model;

public sealed class MenuCategory
{
    public MenuCategory(string name, string description)
    {
        Id = Guid.NewGuid();

        Update(name, description);

        MenuItems = new List<MenuItem>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid MenuId { get; set; }

    public Menu Menu { get; set; }
    public ICollection<MenuItem> MenuItems { get; set; }

    public void Update(string name, string description)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name is required");

        Name = name;
        Description = description;
    }

    public void AddItem(MenuItem item)
    {
        MenuItems.Add(item);
    }
}