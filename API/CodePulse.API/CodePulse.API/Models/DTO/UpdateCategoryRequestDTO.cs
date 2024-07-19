namespace CodePulse.API.Models.DTO;

//POR QUÉ REALIZAR ESTE DTO SI EL DE CREACION ES IGUAL A ESTE?
//RESPUESTA: ESTO SE DEBE A CONVENCIONES DE CLEAN CODE.\

public class UpdateCategoryRequestDTO
{
    public string Name { get; set; }
    public string UrlHandle { get; set; }
    


    public UpdateCategoryRequestDTO(string name, string urlHandle)
    {
        Name = name;
        UrlHandle = urlHandle;
    }
}