using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FastDinner.Infrastructure.Persistence.Repositories;

public class MenuRepository : BaseRepository<Menu>, IMenuRepository
{
    public MenuRepository(DinnerContext context) : base(context)
    {

    }

    public Task<List<MenuCategory>> GetAllCategories(Guid menuId)
    {
        return Context.Menus.Where(x => x.Id == menuId)
            .SelectMany(x => x.Categories)
            .ToListAsync();
    }

    public override Task<Menu> GetByIdAsync(Guid id)
    {
        return Context.Menus
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}