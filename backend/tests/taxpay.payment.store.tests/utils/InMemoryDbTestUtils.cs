using Microsoft.EntityFrameworkCore;
using taxpay.payment.store.contexts;

namespace taxpay.payment.store.tests.utils;

public class InMemoryDbTestUtils
{
    public static InMemoryDbContext GetInMemoryContext(string testDbName)
    {
        var options = new DbContextOptionsBuilder<InMemoryDbContext>()
            .UseInMemoryDatabase(testDbName)
            .Options;

        return new InMemoryDbContext(options);
    }
}
