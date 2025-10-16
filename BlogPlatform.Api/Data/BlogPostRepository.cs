using BlogPlatform.Api.Models.Entities;

namespace BlogPlatform.Api.Data;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly AppDbContext _db;
    public BlogPostRepository(AppDbContext db) => _db = db;

    public IQueryable<BlogPost> Query() => _db.BlogPosts.AsQueryable();

    public async Task AddAsync(BlogPost post) => await _db.BlogPosts.AddAsync(post);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
