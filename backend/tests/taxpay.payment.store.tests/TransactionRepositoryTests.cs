using taxpay.payment.store;
using taxpay.payment.store.contexts;
using taxpay.payment.store.models;
using taxpay.payment.store.tests.utils;

public class TransactionRepositoryTests : IAsyncLifetime
{
    private readonly TransactionRepository _transactionRepository;
    private readonly InMemoryDbContext _inMemoryDbContext;

    public TransactionRepositoryTests()
    {
        _inMemoryDbContext = InMemoryDbTestUtils.GetInMemoryContext(nameof(TransactionRepositoryTests));
        _transactionRepository = new TransactionRepository(_inMemoryDbContext);
    }
    public Task InitializeAsync()
    {
        _inMemoryDbContext.Transactions.RemoveRange(_inMemoryDbContext.Transactions);
        return _inMemoryDbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _inMemoryDbContext.DisposeAsync();
    }

    [Fact]
    public async Task CanAddAndRetrieveTransaction()
    {
        // Arrange
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };
        
        var sourceAccountName = "sourceAccountName";
        var sourceAccountBalance = 100;
        var sourceAccountCreatedDate = DateTime.UtcNow;
        var sourceAccount = new Account { Name = sourceAccountName, Balance = sourceAccountBalance, CreatedDate = sourceAccountCreatedDate };

        var destinationAccountName = "destinationAccountName";
        var destinationAccountBalance = 200;
        var destinationAccountCreatedDate = DateTime.UtcNow;
        var destinationAccount = new Account { Name = destinationAccountName, Balance = destinationAccountBalance, CreatedDate = destinationAccountCreatedDate };

        var amount = 50;
        var transactionCreatedDate = DateTime.UtcNow;
        var transaction = new Transaction { Accountant = accountant, Date = transactionCreatedDate, SourceAccount = sourceAccount, DestinationAccount = destinationAccount, Amount = amount };

        // Act
        _inMemoryDbContext.Accountants.Add(accountant);
        _inMemoryDbContext.Accounts.Add(sourceAccount);
        _inMemoryDbContext.Accounts.Add(destinationAccount);
        _inMemoryDbContext.Transactions.Add(transaction);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedTransaction = await _transactionRepository.FindByIdAsync(transaction.Id);

        // Assert
        Assert.NotNull(retrievedTransaction);
        Assert.Equal(sourceAccountName, retrievedTransaction.SourceAccount.Name);
        Assert.Equal(sourceAccount.Id, retrievedTransaction.SourceAccount.Id);
        Assert.Equal(destinationAccountName, retrievedTransaction.DestinationAccount.Name);
        Assert.Equal(destinationAccount.Id, retrievedTransaction.DestinationAccount.Id);
        Assert.Equal(firstName, retrievedTransaction.Accountant.FirstName);
        Assert.Equal(lastName, retrievedTransaction.Accountant.LastName);
        Assert.Equal(accountant.Id, retrievedTransaction.Accountant.Id);
        Assert.Equal(amount, retrievedTransaction.Amount);
        Assert.Equal(transactionCreatedDate, retrievedTransaction.Date);
    }

    [Fact]
    public async Task WhenTransactionDoesNotExistRetrieveNull()
    {
        // Arrange
        var fakeId = -1;
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };
        
        var sourceAccountName = "sourceAccountName";
        var sourceAccountBalance = 100;
        var sourceAccountCreatedDate = DateTime.UtcNow;
        var sourceAccount = new Account { Name = sourceAccountName, Balance = sourceAccountBalance, CreatedDate = sourceAccountCreatedDate };

        var destinationAccountName = "destinationAccountName";
        var destinationAccountBalance = 200;
        var destinationAccountCreatedDate = DateTime.UtcNow;
        var destinationAccount = new Account { Name = destinationAccountName, Balance = destinationAccountBalance, CreatedDate = destinationAccountCreatedDate };

        var amount = 50;
        var transactionCreatedDate = DateTime.UtcNow;
        var transaction = new Transaction { Accountant = accountant, Date = transactionCreatedDate, SourceAccount = sourceAccount, DestinationAccount = destinationAccount, Amount = amount };

        // Act
        _inMemoryDbContext.Accountants.Add(accountant);
        _inMemoryDbContext.Accounts.Add(sourceAccount);
        _inMemoryDbContext.Accounts.Add(destinationAccount);
        _inMemoryDbContext.Transactions.Add(transaction);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedTransaction = await _transactionRepository.FindByIdAsync(fakeId);
        // Assert
        Assert.Null(retrievedTransaction);
    }

    [Fact]
    public async Task ShouldRetrieveAllTransactions()
    {
        // Arrange
        var expectedNumberOfTransactions = 2;
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };
        
        var sourceAccountName = "sourceAccountName";
        var sourceAccountBalance = 100;
        var sourceAccountCreatedDate = DateTime.UtcNow;
        var sourceAccount = new Account { Name = sourceAccountName, Balance = sourceAccountBalance, CreatedDate = sourceAccountCreatedDate };

        var destinationAccountName = "destinationAccountName";
        var destinationAccountBalance = 200;
        var destinationAccountCreatedDate = DateTime.UtcNow;
        var destinationAccount = new Account { Name = destinationAccountName, Balance = destinationAccountBalance, CreatedDate = destinationAccountCreatedDate };

        var transaction1Amount = 50;
        var transaction1CreatedDate = DateTime.UtcNow;
        var transaction1 = new Transaction { Accountant = accountant, Date = transaction1CreatedDate, SourceAccount = sourceAccount, DestinationAccount = destinationAccount, Amount = transaction1Amount };

        var transaction2Amount = -100;
        var transaction2CreatedDate = DateTime.UtcNow;
        var transaction2 = new Transaction { Accountant = accountant, Date = transaction2CreatedDate, SourceAccount = sourceAccount, DestinationAccount = destinationAccount, Amount = transaction2Amount };

        // Act
        _inMemoryDbContext.Accountants.Add(accountant);
        _inMemoryDbContext.Accounts.Add(sourceAccount);
        _inMemoryDbContext.Accounts.Add(destinationAccount);
        _inMemoryDbContext.Transactions.Add(transaction1);
        _inMemoryDbContext.Transactions.Add(transaction2);
        await _inMemoryDbContext.SaveChangesAsync();


        var retrievedTransactions = await _transactionRepository.FindAllAsync();

        // Assert
        Assert.NotNull(retrievedTransactions);
        Assert.Equal(retrievedTransactions.Count, expectedNumberOfTransactions);
    }

    [Fact]
    public async Task ShouldRetrieveAnEmptyListWhereNoAccountsExists()
    {
        // Arrange
        var expectedNumberOfTransactions = 0;
        var retrievedTransactions = await _transactionRepository.FindAllAsync();

        // Assert
        Assert.NotNull(retrievedTransactions);
        Assert.Equal(retrievedTransactions.Count, expectedNumberOfTransactions);
    }

    [Fact]
    public async Task CanCreateATransaction()
    {
        // Arrange
        var expectedNumberOfTransactions = 1;
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };
        
        var sourceAccountName = "sourceAccountName";
        var sourceAccountBalance = 100;
        var sourceAccountCreatedDate = DateTime.UtcNow;
        var sourceAccount = new Account { Name = sourceAccountName, Balance = sourceAccountBalance, CreatedDate = sourceAccountCreatedDate };

        var destinationAccountName = "destinationAccountName";
        var destinationAccountBalance = 200;
        var destinationAccountCreatedDate = DateTime.UtcNow;
        var destinationAccount = new Account { Name = destinationAccountName, Balance = destinationAccountBalance, CreatedDate = destinationAccountCreatedDate };

        var transactionAmount = 50;
        var transactionCreatedDate = DateTime.UtcNow;
        var transaction = new Transaction { Accountant = accountant, Date = transactionCreatedDate, SourceAccount = sourceAccount, DestinationAccount = destinationAccount, Amount = transactionAmount };

        // Act
        _inMemoryDbContext.Accountants.Add(accountant);
        _inMemoryDbContext.Accounts.Add(sourceAccount);
        _inMemoryDbContext.Accounts.Add(destinationAccount);
        
        await _transactionRepository.CreateAsync(transaction);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedTransactions = await _transactionRepository.FindAllAsync();

        var retrievedTransaction = retrievedTransactions.First();

        // Assert
        Assert.NotNull(retrievedTransactions);
        Assert.Equal(expectedNumberOfTransactions, retrievedTransactions.Count);
        Assert.Equal(transactionAmount, retrievedTransaction.Amount);
        Assert.Equal(transactionCreatedDate, retrievedTransaction.Date);
        Assert.Equal(sourceAccountName, retrievedTransaction.SourceAccount.Name);
        Assert.Equal(sourceAccount.Id, retrievedTransaction.SourceAccount.Id);
        Assert.Equal(destinationAccountName, retrievedTransaction.DestinationAccount.Name);
        Assert.Equal(destinationAccount.Id, retrievedTransaction.DestinationAccount.Id);
    }
}