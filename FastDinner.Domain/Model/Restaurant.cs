namespace FastDinner.Domain.Model;

public class Restaurant
{
    public Restaurant(string name, string address, string phone, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;

        Receptionists = new List<Receptionist>();
        Menus = new List<Menu>();
        Tables = new List<Table>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public virtual ICollection<Receptionist> Receptionists { get; set; }
    public virtual ICollection<Table> Tables { get; set; }
    public virtual ICollection<Menu> Menus { get; set; }
}