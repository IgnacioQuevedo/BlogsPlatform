using System.Runtime.CompilerServices;
using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Category> CreateAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
       return await _dbContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetById(Guid id)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Category?> UpdateAsync(Category category)
    {

        var categoryWithoutChanges = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

        if (categoryWithoutChanges != null)
        {
            _dbContext.Entry(categoryWithoutChanges).CurrentValues.SetValues(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        return null;
    }

    public async Task<Category?> DeleteCategoryAsync(Guid id)
    {
        var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (existingCategory != null)
        {
            _dbContext.Categories.Remove(existingCategory);
            
            //Sin la linea de abajo, no se borra el Id de la category a borrar.
            //Con esta linea le hacemos entender al E.F que realmente queremos borrarla por completo
            _dbContext.Entry(existingCategory).State = EntityState.Deleted;
            
            await _dbContext.SaveChangesAsync();
            return existingCategory;
        }
        
        return null;
    }
}