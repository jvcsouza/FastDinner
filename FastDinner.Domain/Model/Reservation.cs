namespace FastDinner.Domain.Model;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid TableId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime Date { get; set; }
    public bool IsCancel { get; set; }
    public DateTime? CheckInDate { get; set; }

    public virtual Customer Customer { get; set; }
    public virtual Table Table { get; set; }

    public void CancelReservation()
    {
        IsCancel = true;
    }

    public void ChangeReservationDate(DateTime newDate)
    {
        Date = newDate;
    }
}