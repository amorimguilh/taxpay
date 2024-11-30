using taxpay.payment.store;
using taxpay.payment.store.contexts;
using taxpay.payment.store.models;
using taxpay.payment.store.tests.utils;

public class AccountRepositoryTests : IAsyncLifetime
{
    private readonly AccountRepository _accountRepository;
    private readonly InMemoryDbContext _inMemoryDbContext;

    public AccountRepositoryTests()
    {
        _inMemoryDbContext = InMemoryDbTestUtils.GetInMemoryContext(nameof(AccountRepositoryTests));
        _accountRepository = new AccountRepository(_inMemoryDbContext);
    }
    public Task InitializeAsync()
    {
        _inMemoryDbContext.Accounts.RemoveRange(_inMemoryDbContext.Accounts);
        return _inMemoryDbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _inMemoryDbContext.DisposeAsync();
    }

    [Fact]
    public async Task CanAddAndRetrieveAccount()
    {
        // Arrange
        var accountName = "accountName";
        var balance = 100;
        var createdDate = DateTime.UtcNow;
        var account = new Account { Name = accountName, Balance = balance, CreatedDate = createdDate };

        // Act
        _inMemoryDbContext.Accounts.Add(account);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedAccount = await _accountRepository.FindByIdAsync(account.Id);

        // Assert
        Assert.NotNull(retrievedAccount);
        Assert.Equal(accountName, retrievedAccount.Name);
        Assert.Equal(balance, retrievedAccount.Balance);
        Assert.Equal(createdDate, retrievedAccount.CreatedDate);
    }

    [Fact]
    public async Task WhenAccountDoesNotExistRetrieveNull()
    {
        // Arrange
        var accountName = "accountName";
        var balance = 100;
        var createdDate = DateTime.UtcNow;
        var fakeId = -1;
        var account = new Account { Name = accountName, Balance = balance, CreatedDate = createdDate };

        // Act
        _inMemoryDbContext.Accounts.Add(account);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedAccount = await _accountRepository.FindByIdAsync(fakeId);

        // Assert
        Assert.Null(retrievedAccount);
    }

    [Fact]
    public async Task ShouldRetrieveAllAccounts()
    {
        // Arrange
        var expectedNumberOfAccounts = 2;
        var account1Name = "account1Name";
        var account1Balance = 100;
        var account1CreatedDate = DateTime.UtcNow;
        var account1 = new Account { Name = account1Name, Balance = account1Balance, CreatedDate = account1CreatedDate };

        var account2Name = "account2Name";
        var account2Balance = 50;
        var account2CreatedDate = DateTime.UtcNow;
        var account2 = new Account { Name = account2Name, Balance = account2Balance, CreatedDate = account2CreatedDate };

        // Act
        _inMemoryDbContext.Accounts.Add(account1);
        _inMemoryDbContext.Accounts.Add(account2);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedAccounts = await _accountRepository.FindAllAsync();

        // Assert
        Assert.NotNull(retrievedAccounts);
        Assert.Equal(retrievedAccounts.Count, expectedNumberOfAccounts);
    }

    [Fact]
    public async Task ShouldRetrieveAnEmptyListWhereNoAccountsExists()
    {
        // Arrange
        var expectedNumberOfAccountants = 0;
        var retrievedAccounts = await _accountRepository.FindAllAsync();

        // Assert
        Assert.NotNull(retrievedAccounts);
        Assert.Equal(retrievedAccounts.Count, expectedNumberOfAccountants);
    }

    [Fact]
    public async Task CanCreateAnAccount()
    {
        // Arrange
        var expectedNumberOfAccounts = 1;
        var account1Name = "account1Name";
        var account1Balance = 100;
        var account1CreatedDate = DateTime.UtcNow;
        var account1 = new Account { Name = account1Name, Balance = account1Balance, CreatedDate = account1CreatedDate };

        // Act
        await _accountRepository.CreateAsync(account1);
        await _accountRepository.SaveAsync();
        var retrievedAccounts = await _accountRepository.FindAllAsync();

        var retrievedAccount = retrievedAccounts.First();

        // Assert
        Assert.NotNull(retrievedAccounts);
        Assert.Equal(expectedNumberOfAccounts, retrievedAccounts.Count);
        Assert.Equal(account1Name, retrievedAccount.Name);
        Assert.Equal(account1Balance, retrievedAccount.Balance);
        Assert.Equal(account1CreatedDate, retrievedAccount.CreatedDate);
    }

    [Fact]
    public async Task CanUpdateAnAccountant()
    {
        // Arrange
        var expectedNumberOfAccounts = 1;
        var account1Name = "account1Name";
        var account1Balance = 100;
        var account1CreatedDate = DateTime.UtcNow;
        var newAccount1Balance = 50;
        var account1 = new Account { Name = account1Name, Balance = account1Balance, CreatedDate = account1CreatedDate };

        // Act
        await _accountRepository.CreateAsync(account1);
        await _accountRepository.SaveAsync();
        var retrievedAccounts = await _accountRepository.FindAllAsync();

        var retrievedAccount = retrievedAccounts.First();
        retrievedAccount.Balance = newAccount1Balance;

        await _accountRepository.UpdateAsync(retrievedAccount);
        await _accountRepository.SaveAsync();

        // Assert
        Assert.NotNull(retrievedAccounts);
        Assert.Equal(expectedNumberOfAccounts, retrievedAccounts.Count);
        Assert.Equal(account1Name, retrievedAccount.Name);
        Assert.Equal(account1CreatedDate, retrievedAccount.CreatedDate);
        Assert.Equal(newAccount1Balance, retrievedAccount.Balance);
    }

    [Fact]
    public async Task CanDeleteAnAccountant()
    {
        // Arrange
        var expectedNumberOfAccounts = 0;
        var account1Name = "account1Name";
        var account1Balance = 100;
        var account1CreatedDate = DateTime.UtcNow;
        var account1 = new Account { Name = account1Name, Balance = account1Balance, CreatedDate = account1CreatedDate };

        // Act
        await _accountRepository.CreateAsync(account1);
        await _accountRepository.SaveAsync();

        var retrievedAccounts = await _accountRepository.FindAllAsync();
        var retrievedAccount = retrievedAccounts.First();

        await _accountRepository.DeleteAsync(retrievedAccount.Id);
        await _accountRepository.SaveAsync();

        retrievedAccounts = await _accountRepository.FindAllAsync(); 

        // Assert
        Assert.NotNull(retrievedAccounts);
        Assert.Equal(expectedNumberOfAccounts, retrievedAccounts.Count);
    }
}

/*
 * Why the unique email constraint was not tested, because of the limitations of the in-memory database
 * Perhaps one of the improvements would be to use something like test containers to use a real database and have the 
 * tests to be more assertive
 */