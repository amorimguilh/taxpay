using taxpay.payment.store.models;

namespace taxpay.payment.store.interfaces;

public interface ITransactionRepository
{
    Task CreateAsync(Transaction transaction);
    Task<List<Transaction>> FindAllAsync();
    Task<Transaction?> FindByIdAsync(int id);
}