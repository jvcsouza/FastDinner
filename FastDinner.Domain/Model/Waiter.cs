namespace FastDinner.Domain.Model;

public class Waiter : Employee
{
    public Waiter()
    {
        Role = EmployeeType.Waiter;
    }

    public void CreateOrder()
    {

    }
}