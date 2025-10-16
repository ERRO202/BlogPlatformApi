using BlogPlatform.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogPlatform.Tests.Helpers;

public static class TestDb
{
    // Creates a fresh, isolated in-memory DB per test
    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;
        return new AppDbContext(options);
    }
}
