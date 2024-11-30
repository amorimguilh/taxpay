using taxpay.payment.store.models;

namespace taxpay.payment.store.interfaces;

public interface IAccountantRepository 
{
    Task CreateAsync(Accountant accountant);
    Task UpdateAsync(Accountant accountant);
    Task DeleteAsync(int id);
    Task<List<Accountant>> FindAllAsync();
    Task<Accountant?> FindByIdAsync(int id);
}
