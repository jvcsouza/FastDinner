namespace FastDinner.Domain.Model;

public class MenuItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid MenuId { get; set; }
    public Guid ProductId { get; set; }
    
    public virtual Menu Menu { get; set; }
    public virtual Product Product { get; set; }
}