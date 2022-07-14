using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FastDinner.Infrastructure.Persistence.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly DinnerContext _context;

    public MenuRepository(DinnerContext context)
    {
        _context = context;
    }

    public async Task<Menu> GetById(Guid id)
    {
        return await _context.Menus.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Menu> Create(Menu menu)
    {
        await _context.Menus.AddAsync(menu);
        return menu;
    }

    public async Task<IEnumerable<Menu>> GetAll()
    {
        return await _context.Menus.ToListAsync();
    }

    public Task<Menu> Update(Menu menu)
    {
        return Task.FromResult(_context.Set<Menu>().Update(menu).Entity);
    }
}