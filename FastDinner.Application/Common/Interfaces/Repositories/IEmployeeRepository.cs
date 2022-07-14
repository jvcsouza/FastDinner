using FastDinner.Domain.Model;

namespace FastDinner.Application.Common.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee> GetByEmail(string email);
}