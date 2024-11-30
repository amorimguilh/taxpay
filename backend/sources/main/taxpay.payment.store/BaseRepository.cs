using taxpay.payment.store.contexts;
using taxpay.payment.store.interfaces;

namespace taxpay.payment.store;

public class BaseRepository : IBaseRepository
{
    private readonly InMemoryDbContext _context;

    public BaseRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
