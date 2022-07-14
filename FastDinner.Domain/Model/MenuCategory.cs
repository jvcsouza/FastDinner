namespace FastDinner.Domain.Model;

public class MenuCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid MenuId { get; set; }
    
    public virtual Menu Menu { get; set; }
    public ICollection<MenuItem> MenuItems { get; set; }
}