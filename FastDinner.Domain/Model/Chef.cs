namespace FastDinner.Domain.Model;

public class Chef : Employee
{
    public Chef()
    {
        Role = EmployeeType.Chef;
    }

    public void TakeOrder()
    {
        
    }
}