using Microsoft.EntityFrameworkCore;
using taxpay.payment.store.contexts;
using taxpay.payment.store.interfaces;
using taxpay.payment.store.models;

namespace taxpay.payment.store;

public class TransactionRepository : BaseRepository, ITransactionRepository
{
    private readonly InMemoryDbContext _context;

    public TransactionRepository(InMemoryDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task CreateAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public async Task<List<Transaction>> FindAllAsync()
    {
        var transactions = await _context.Transactions
            .AsNoTracking()
            .Include(model => model.SourceAccount)
            .Include(model => model.DestinationAccount)
            .Include(model => model.Accountant)
            .ToListAsync();
        if (transactions == null) 
        {
            return new List<Transaction>();
        }
        return transactions;
    }

    public async Task<Transaction?> FindByIdAsync(int id)
    {
        var transactions = await _context.Transactions
            .AsNoTracking()
            .Include(model => model.SourceAccount)
            .Include(model => model.DestinationAccount)
            .Include(model => model.Accountant)
            .FirstOrDefaultAsync(transaction => transaction.Id == id);

        return transactions;
    }
}

/*
 * Does not make sense to have an update and delete methods in transactions table since it will hold
 * history information to what happened in an account
 *
 * I included the models here to be retrieved in the query just for test and validation purposes
 */