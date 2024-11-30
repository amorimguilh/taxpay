using taxpay.payment.store.models;

namespace taxpay.payment.store.interfaces;

public interface IAccountRepository 
{
    Task CreateAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(int id);
    Task<List<Account>> FindAllAsync();
    Task<Account?> FindByIdAsync(int id);
}
