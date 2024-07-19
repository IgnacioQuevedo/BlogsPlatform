using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation;

public class BlogPostsRepository: IBlogPostRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BlogPostsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<BlogPost> CreateAsync(BlogPost blogToCreate)
    {

        await _dbContext.BlogPosts.AddAsync(blogToCreate);
        await _dbContext.SaveChangesAsync();
        return blogToCreate;
    }

    public async Task<IEnumerable<BlogPost>> GetAllBlogsAsync()
    {
        //Le estoy diciendo a Entity Framework que también cargue las categorías asociadas a
        //cada uno de  los blogs.
        return await _dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
    }

    public async Task<BlogPost?> GetBlogByIdAsync(Guid id)
    {

        return await _dbContext.BlogPosts.Include(x=>x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id);
        
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogWithUpdates)
    {

        var blogPostInDb = await _dbContext.BlogPosts.Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == blogWithUpdates.Id);

        if (blogPostInDb != null)
        {
            //Update BlogPost
            _dbContext.BlogPosts.Entry(blogPostInDb).CurrentValues.SetValues(blogWithUpdates);
            
            //Update Categories
            blogPostInDb.Categories = blogWithUpdates.Categories;
            
            await _dbContext.SaveChangesAsync();
            return blogWithUpdates;
        }

        return null;
    }

    public async Task<BlogPost?> DeleteAsync(Guid idToDelete)
    {
       var blogPostToDelete = await _dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == idToDelete);
       
       if (blogPostToDelete != null)
       {
           _dbContext.BlogPosts.Remove(blogPostToDelete);
           _dbContext.Entry(blogPostToDelete).State = EntityState.Deleted;
           
           await _dbContext.SaveChangesAsync();
           return blogPostToDelete;
       }

       return null;

    }
    
    
    public async Task<BlogPost?> GetBlogByUrlHandleAsync(string urlHandle)
    {
        return await _dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle.Equals(urlHandle));

    }
    
}