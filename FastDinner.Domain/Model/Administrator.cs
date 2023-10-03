namespace FastDinner.Domain.Model;

public class Administrator : Employee
{
    public Administrator()
    {
        Role = EmployeeType.Admin;
    }
}