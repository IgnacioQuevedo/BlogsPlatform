using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface;

public interface ICategoryRepository
{
    Task<Category> CreateAsync(Category category);
    Task<IEnumerable<Category>> GetAllAsync();

    //Es nullable porque se puede conseguir una category o null si no encuentra (Por eso el ?)
    Task<Category?> GetById(Guid Id);

    Task<Category?> UpdateAsync(Category category);
    
    Task<Category?> DeleteCategoryAsync (Guid id);
}