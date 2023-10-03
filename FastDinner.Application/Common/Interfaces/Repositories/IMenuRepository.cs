using FastDinner.Domain.Model;

namespace FastDinner.Application.Common.Interfaces.Repositories;

public interface IMenuRepository : IRepository<Menu>
{
    Task<List<MenuCategory>> GetAllCategories(Guid menuId);
}