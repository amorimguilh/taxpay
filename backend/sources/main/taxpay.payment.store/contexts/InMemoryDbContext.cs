using Microsoft.EntityFrameworkCore;
using taxpay.payment.store.models;

namespace taxpay.payment.store.contexts;

public class InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Accountant> Accountants { get; set; } = null!;
}