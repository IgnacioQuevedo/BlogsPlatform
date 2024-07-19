namespace CodePulse.API.Models.DTO;

public class CreateCategoryRequestDTO
{
    public string Name { get; set; }
    public string UrlHandle { get; set; }
    


    public CreateCategoryRequestDTO(string name, string urlHandle)
    {
        Name = name;
        UrlHandle = urlHandle;
    }
}