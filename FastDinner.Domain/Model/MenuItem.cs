namespace FastDinner.Domain.Model;

public class MenuItem
{
    protected MenuItem()
    {
    }

    public MenuItem(Product product, decimal price, string name, string description)
    {
        Id = Guid.NewGuid();
        Product = product;
        Price = price;
        Name = name;
        Description = description;

        Validate();
    }

    public MenuItem(Product product, string description, decimal price)
    {
        Id = Guid.NewGuid();
        Product = product;
        Price = price;
        Name = product?.Name;
        Description = description;

        Validate();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid ProductId { get; set; }

    public virtual Product Product { get; set; }

    private void Validate()
    {
        if (string.IsNullOrEmpty(Name))
            throw new ArgumentException("Name is required");

        if (Price <= 0)
            throw new ArgumentException("Price must be greater than 0");

        if (Product is null)
            throw new ArgumentException("Product is required");
    }
}