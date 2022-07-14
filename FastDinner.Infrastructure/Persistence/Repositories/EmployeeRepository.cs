using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FastDinner.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DinnerContext _context;

    public EmployeeRepository(DinnerContext context)
    {
        _context = context;
    }

    public Task<Employee> Create(Employee entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Employee>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetByEmail(string email)
    {
        return _context.Employees.FirstOrDefaultAsync(x => x.Email == email);
    }

    public Task<Employee> GetById(Guid id)
    {
        return _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Employee> Update(Employee entity)
    {
        throw new NotImplementedException();
    }
}