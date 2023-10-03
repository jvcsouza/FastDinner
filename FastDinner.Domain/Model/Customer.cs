namespace FastDinner.Domain.Model;

public class Customer 
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}