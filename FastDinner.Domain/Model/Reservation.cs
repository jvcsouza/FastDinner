using FastDinner.Domain.Contracts;

namespace FastDinner.Domain.Model;

public sealed class Reservation : Entity, IRestaurant
{
    private Reservation() { }

    public Reservation(Customer customer, Table table, DateTime checkInDate)
    {
        Id = Guid.NewGuid();
        Customer = customer;
        Table = table;
        CheckInDate = checkInDate;
    }

    public Guid TableId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTime Date { get; set; }
    public bool IsCancel { get; set; }
    public DateTime? CheckInDate { get; set; }

    public Customer Customer { get; set; }
    public Table Table { get; set; }
    public Restaurant Restaurant { get; set; }

    public void CancelReservation()
    {
        IsCancel = true;
    }

    public void ChangeReservationDate(DateTime newDate)
    {
        Date = newDate;
    }
}