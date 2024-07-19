using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        //We realize this aiming to evit to create a new instance.
        // What I mean is: ICategoryRepository a = new etc etc etc
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        //Primer endPoint
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDTO request)
        {
            // Map DTO to Domain Model
            Category category = new Category(request.Name, request.UrlHandle);
            await _categoryRepository.CreateAsync(category);

            //Domain model to DTO
            var response = new CategoryDTO(category.Id, category.Name, category.UrlHandle);
            return Ok(response);
        }

        //PARA ACCEDER AL ENDPOINT DEL GET VAMOS AL URL: http://localhost:5214/api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] Guid id)
        {
            var categories = await _categoryRepository.GetAllAsync();
            
            //Map domain model into DTO, cause we can t show aspects of the Business Logic

            var response = new List<CategoryDTO>();
            
                
            foreach (var category in categories)
            {
                
                CategoryDTO newDto = new CategoryDTO(category.Id,category.Name,category.UrlHandle);
                response.Add(newDto);
            }

            return Ok(response);
        }
        
        //GET BY ID : http://localhost:5214/api/categories/{id}
        //TO TEST IT IN THE API ONLY WITH THIS IS ENOUGH
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var categoryFound = await _categoryRepository.GetById(id);

            if (categoryFound is null)
            {
                return NotFound();
            }

            CategoryDTO response = new CategoryDTO(categoryFound.Id, categoryFound.Name, categoryFound.UrlHandle);
            
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategoryRequestDTO request)
        {
            Category categoryWithUpdates = new Category(request.Name, request.UrlHandle);
            categoryWithUpdates.Id = id;

            categoryWithUpdates = await _categoryRepository.UpdateAsync(categoryWithUpdates);
            if (categoryWithUpdates == null)
            {
                return NotFound();
            }

            CategoryDTO response = new CategoryDTO(categoryWithUpdates.Id, categoryWithUpdates.Name, categoryWithUpdates.UrlHandle);
            return Ok(response);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _categoryRepository.DeleteCategoryAsync(id);
            if (category != null)
            {
                var response = new CategoryDTO(category.Id, category.Name, category.UrlHandle);
                return Ok(response);
            }

            return NotFound();
        }
    }
}