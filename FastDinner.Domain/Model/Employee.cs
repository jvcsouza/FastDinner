namespace FastDinner.Domain.Model;

public abstract class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    // public string Username { get; set; }
    public string Password { get; set; }
    public EmployeeType Role { get; protected set; }
    public bool Act { get; set; }
    public Guid RestaurantId { get; set; }
    public virtual Restaurant Restaurant { get; set; }
}