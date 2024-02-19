namespace FastDinner.Domain.Model;

public sealed class Restaurant : Entity
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
        Reservations = new List<Reservation>();
    }

    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public ICollection<Receptionist> Receptionists { get; set; }
    public ICollection<Table> Tables { get; set; }
    public ICollection<Menu> Menus { get; set; }
    public ICollection<Reservation> Reservations { get; set; }

    public void Update(string name, string address, string phone, string email)
    {
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
    }

    public Menu IncludeMenu(string name, string description, string image)
    {
        var menu = new Menu(name, description, image, false);

        Menus.Add(menu);

        return menu;
    }
}