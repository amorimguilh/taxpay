using taxpay.payment.store;
using taxpay.payment.store.contexts;
using taxpay.payment.store.models;
using taxpay.payment.store.tests.utils;

public class AccountantRepositoryTests : IAsyncLifetime
{
    private readonly AccountantRepository _accountantRepository;
    private readonly InMemoryDbContext _inMemoryDbContext;

    public AccountantRepositoryTests()
    {
        _inMemoryDbContext = InMemoryDbTestUtils.GetInMemoryContext(nameof(AccountantRepositoryTests));
        _accountantRepository = new AccountantRepository(_inMemoryDbContext);
    }
    public Task InitializeAsync()
    {
        _inMemoryDbContext.Accountants.RemoveRange(_inMemoryDbContext.Accountants);
        return _inMemoryDbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _inMemoryDbContext.DisposeAsync();
    }

    [Fact]
    public async Task CanAddAndRetrieveAccountant()
    {
        // Arrange
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };

        // Act
        _inMemoryDbContext.Accountants.Add(accountant);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedAccountant = await _accountantRepository.FindByIdAsync(accountant.Id);

        // Assert
        Assert.NotNull(retrievedAccountant);
        Assert.Equal(firstName, retrievedAccountant.FirstName);
        Assert.Equal(lastName, retrievedAccountant.LastName);
        Assert.Equal(email, retrievedAccountant.Email);
    }

    [Fact]
    public async Task WhenAccountantDoesNotExistRetrieveNull()
    {
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var fakeId = -1;
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };

        // Act
        _inMemoryDbContext.Accountants.Add(accountant);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedAccountant = await _accountantRepository.FindByIdAsync(fakeId);

        // Assert
        Assert.Null(retrievedAccountant);
    }

    [Fact]
    public async Task ShouldRetrieveAllAccountants()
    {
        // Arrange
        var expectedNumberOfAccountants = 2;
        var accountant1FirstName = "Accountant1";
        var accountant1LastName = "Accountant1";
        var accountant1Email = "Accountant1@Accountant1.com";
        var accountant1 = new Accountant { FirstName = accountant1FirstName, LastName = accountant1LastName, Email=accountant1Email };

        var accountant2FirstName = "Accountant2";
        var accountant2LastName = "Accountant2";
        var accountant2Email = "Accountant2@Accountant2.com";
        var accountant2 = new Accountant { FirstName = accountant2FirstName, LastName = accountant2LastName, Email=accountant2Email };

        // Act
        _inMemoryDbContext.Accountants.Add(accountant1);
        _inMemoryDbContext.Accountants.Add(accountant2);
        await _inMemoryDbContext.SaveChangesAsync();

        var retrievedAccountants = await _accountantRepository.FindAllAsync();

        // Assert
        Assert.NotNull(retrievedAccountants);
        Assert.Equal(retrievedAccountants.Count, expectedNumberOfAccountants);
    }

    [Fact]
    public async Task ShouldRetrieveAnEmptyListWhereNoAccountantsExists()
    {
        // Arrange
        var expectedNumberOfAccountants = 0;
        var retrievedAccountants = await _accountantRepository.FindAllAsync();

        // Assert
        Assert.NotNull(retrievedAccountants);
        Assert.Equal(retrievedAccountants.Count, expectedNumberOfAccountants);
    }

    [Fact]
    public async Task CanCreateAnAccountant()
    {
        // Arrange
        var expectedNumberOfAccountants = 1;
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };

        // Act
        await _accountantRepository.CreateAsync(accountant);
        await _accountantRepository.SaveAsync();
        var retrievedAccountants = await _accountantRepository.FindAllAsync();

        var retrievedAccountant = retrievedAccountants.First();

        // Assert
        Assert.NotNull(retrievedAccountants);
        Assert.Equal(expectedNumberOfAccountants, retrievedAccountants.Count);
        Assert.Equal(firstName, retrievedAccountant.FirstName);
        Assert.Equal(lastName, retrievedAccountant.LastName);
        Assert.Equal(email, retrievedAccountant.Email);
    }

    [Fact]
    public async Task CanUpdateAnAccountant()
    {
        // Arrange
        var expectedNumberOfAccountants = 1;
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var newFirstName = "NewFirstName";
        var newLastName = "NewLastName";
        var newEmail = "Newtest@test.com";
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };

        // Act
        await _accountantRepository.CreateAsync(accountant);
        await _accountantRepository.SaveAsync();
        
        var retrievedAccountants = await _accountantRepository.FindAllAsync();
        var accountantToUpdate = retrievedAccountants.First();
        accountantToUpdate.FirstName = newFirstName;
        accountantToUpdate.LastName = newLastName;
        accountantToUpdate.Email = newEmail;

        await _accountantRepository.UpdateAsync(accountantToUpdate);
        await _accountantRepository.SaveAsync();


        // Assert
        Assert.NotNull(retrievedAccountants);
        Assert.Equal(expectedNumberOfAccountants, retrievedAccountants.Count);
        Assert.Equal(newFirstName, accountantToUpdate.FirstName);
        Assert.Equal(newLastName, accountantToUpdate.LastName);
        Assert.Equal(newEmail, accountantToUpdate.Email);
    }

    [Fact]
    public async Task CanDeleteAnAccountant()
    {
        // Arrange
        var expectedNumberOfAccountants = 0;
        var firstName = "FirstName";
        var lastName = "LastName";
        var email = "test@test.com";
        var accountant = new Accountant { FirstName = firstName, LastName = lastName, Email=email };

        // Act
        await _accountantRepository.CreateAsync(accountant);
        await _accountantRepository.SaveAsync();
        var retrievedAccountants = await _accountantRepository.FindAllAsync();
        var retrievedAccountant = retrievedAccountants.First();

        await _accountantRepository.DeleteAsync(retrievedAccountant.Id);
        await _accountantRepository.SaveAsync();

        retrievedAccountants = await _accountantRepository.FindAllAsync(); 

        // Assert
        Assert.NotNull(retrievedAccountants);
        Assert.Equal(expectedNumberOfAccountants, retrievedAccountants.Count);
    }
}

/*
 * Why the unique email constraint was not tested, because of the limitations of the in-memory database
 * Perhaps one of the improvements would be to use something like test containers to use a real database and have the 
 * tests to be more assertive
 */