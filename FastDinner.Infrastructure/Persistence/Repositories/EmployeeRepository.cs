using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FastDinner.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(DinnerContext context) : base(context)
    {

    }

    public Task<Employee> GetByEmail(string email)
    {
        return Context.Employees.FirstOrDefaultAsync(x => x.Email == email);
    }
}