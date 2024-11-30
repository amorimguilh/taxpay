using Microsoft.EntityFrameworkCore;
using taxpay.payment.store.contexts;
using taxpay.payment.store.interfaces;
using taxpay.payment.store.models;

namespace taxpay.payment.store;

public class AccountantRepository : BaseRepository, IAccountantRepository
{
    private readonly InMemoryDbContext _context;

    public AccountantRepository(InMemoryDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task CreateAsync(Accountant accountant)
    {
        await _context.Accountants.AddAsync(accountant);
    }

    public async Task DeleteAsync(int id)
    {
        var accountant = await _context.Accountants
            .FirstOrDefaultAsync(accountant => accountant.Id == id);
        
        if(accountant != null)
        {
            _context.Accountants.Remove(accountant);
        }
    }

    public async Task<List<Accountant>> FindAllAsync()
    {
        var accountants = await _context.Accountants.AsNoTracking().ToListAsync();
        if (accountants == null) 
        {
            return new List<Accountant>();
        }
        return accountants;
    }

    public async Task<Accountant?> FindByIdAsync(int id)
    {
        var accountant = await _context.Accountants
            .AsNoTracking()
            .FirstOrDefaultAsync(accountant => accountant.Id == id);

        return accountant;
    }

    public async Task UpdateAsync(Accountant accountant)
    {
        var existingAccountant = await _context.Accountants
        .FirstOrDefaultAsync(accountant => accountant.Id == accountant.Id);

        if (existingAccountant == null)
        {
            throw new KeyNotFoundException($"Accountant with ID {accountant.Id} not found.");
        }

        // Update the properties of the existing accountant
        existingAccountant.FirstName = accountant.FirstName;
        existingAccountant.LastName = accountant.LastName;
        existingAccountant.Email = accountant.Email;
    }
}
