using FastDinner.Domain.Contracts;

namespace FastDinner.Domain.Model;

public class Reservation : Entity, IRestaurant
{
    public Reservation()
    {
        Id = Guid.NewGuid();
    }

    public Guid TableId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTime Date { get; set; }
    public bool IsCancel { get; set; }
    public DateTime? CheckInDate { get; set; }

    public virtual Customer Customer { get; set; }
    public virtual Table Table { get; set; }
    public virtual Restaurant Restaurant { get; set; }

    public void CancelReservation()
    {
        IsCancel = true;
    }

    public void ChangeReservationDate(DateTime newDate)
    {
        Date = newDate;
    }
}