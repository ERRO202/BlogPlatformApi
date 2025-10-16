using BlogPlatform.Api.Models.Entities;

namespace BlogPlatform.Api.Data;

public interface IBlogPostRepository
{
    IQueryable<BlogPost> Query();
    Task AddAsync(BlogPost post);
    Task SaveChangesAsync();
}
