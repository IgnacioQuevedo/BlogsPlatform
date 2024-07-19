using System.Security.Policy;

namespace CodePulse.API.Models.DTO;

public class CategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UrlHandle { get; set; }
    
    public CategoryDTO(Guid categoryId, string categoryName, string categoryUrlHandle)
    {
        Id = categoryId;
        Name = categoryName;
        UrlHandle = categoryUrlHandle;
    }
}