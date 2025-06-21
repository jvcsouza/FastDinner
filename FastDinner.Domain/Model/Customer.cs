namespace FastDinner.Domain.Model;

public class Customer : Entity
{
    public string Name { get; set; }
    public string Document { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}