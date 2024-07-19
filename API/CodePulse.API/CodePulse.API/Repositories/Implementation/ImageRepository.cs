using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation;

public class ImageRepository: IImageRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageRepository(IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }
    
    
    public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
    {
        
        // 1-Upload the image to API/Images)
        
        // TODO ESTO SERÁ A NIVEL LOCAL --------------------------------------------------------------------------->
       
        // Definimos la ruta exacta para el archivo dentro de una variable.
        // Esta ruta se compone por el ContentRoothPath (El proyecto en si), luego irá a la carpeta "Images"
        // Y luego creará un archivo con el fileName y el fileExtension
        var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
            $"{blogImage.FileName}{blogImage.FileExtension}");
        
        //El stream lo que hace es generar un archivo vacio dentro de la ruta solicitada
        await using var stream = new FileStream(localPath, FileMode.Create);
        // Luego, ese archivo vacio le pasaré los valores de mi archivo en si, para eso copio los datos de "file"
        // hacia el stream
        await file.CopyToAsync(stream);
        
        
        // HASTA ACÁ ---------------------------------------------------------------------------> ACÁ ---------------------------------------------------------------------------------------------->
        
        
        //TODO ESTO SERÁ A NIVEL REMOTO --------------------------------------------------------------------------->
       
        // 2-Update the database
        //https://codepulse.com/images/somefilename.jpg

        var httpRequest = _httpContextAccessor.HttpContext.Request;
        var urlPath =
            $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/images/" +
            $"{blogImage.FileName}{blogImage.FileExtension}";

        blogImage.Url = urlPath;
        await _dbContext.BlogImages.AddAsync(blogImage);
        await _dbContext.SaveChangesAsync();

        return blogImage;

    }

    public async Task<IEnumerable<BlogImage>> GetAll()
    {
        return await _dbContext.BlogImages.ToListAsync();
    }
    
    
    
}