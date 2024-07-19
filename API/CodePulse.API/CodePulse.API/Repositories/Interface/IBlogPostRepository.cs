using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;

namespace CodePulse.API.Repositories.Interface;

public interface IBlogPostRepository
{

    Task<BlogPost> CreateAsync(BlogPost blogToCreate);
    Task<IEnumerable<BlogPost>> GetAllBlogsAsync();
    Task<BlogPost?> GetBlogByIdAsync(Guid id);
    Task<BlogPost?> UpdateAsync(BlogPost blogWithUpdates);
    Task<BlogPost?> DeleteAsync(Guid id);
    Task<BlogPost?> GetBlogByUrlHandleAsync(string urlHandle);
}
