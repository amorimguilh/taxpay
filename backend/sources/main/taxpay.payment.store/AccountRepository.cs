using Microsoft.EntityFrameworkCore;
using taxpay.payment.store.contexts;
using taxpay.payment.store.interfaces;
using taxpay.payment.store.models;

namespace taxpay.payment.store;

public class AccountRepository : BaseRepository, IAccountRepository
{
    private readonly InMemoryDbContext _context;

    public AccountRepository(InMemoryDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task CreateAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public async Task DeleteAsync(int id)
    {
        var account = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Id == id);
        
        if(account != null)
        {
            _context.Accounts.Remove(account);
        }
    }

    public async Task<List<Account>> FindAllAsync()
    {
        var accounts = await _context.Accounts.AsNoTracking().ToListAsync();
        if (accounts == null) 
        {
            return new List<Account>();
        }
        return accounts;
    }

    public async Task<Account?> FindByIdAsync(int id)
    {
        var accounts = await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(account => account.Id == id);

        return accounts;
    }

    public async Task UpdateAsync(Account account)
    {
        var existingAccount = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Id == account.Id);

        if (account == null)
        {
            throw new KeyNotFoundException($"Account with ID {account!.Id} not found.");
        }

        // Update only the needed properties
        existingAccount!.Balance = account.Balance;
    }
}

