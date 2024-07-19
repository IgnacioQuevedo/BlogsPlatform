using System.Security.Policy;

namespace CodePulse.API.Models.Domain;

public class Category
{
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UrlHandle { get; set; }
    
    //PARA EL ENTITY FRAMEWORK (RELACION N-N CON BLOGPOST)
    public ICollection<BlogPost> BlogPosts { get; set; }
    
    public Category(string name, string urlHandle)
    {
        Name = name;
        UrlHandle = urlHandle;
    }
    
    public Category(Guid id, string name, string urlHandle)
    {
        Id = id;
        Name = name;
        UrlHandle = urlHandle;
    }
    
}