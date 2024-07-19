using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }


        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDTO createBlogFromFrontEnd)
        {
            //Convert from DTO to Domain object
            BlogPost blogToCreate = new BlogPost()
            {
                
                Title = createBlogFromFrontEnd.Title,
                ShortDescription = createBlogFromFrontEnd.ShortDescription,
                Content = createBlogFromFrontEnd.Content,
                FeaturedImageUrl = createBlogFromFrontEnd.FeaturedImageUrl,
                UrlHandle = createBlogFromFrontEnd.UrlHandle,
                PublishedDate = createBlogFromFrontEnd.PublishedDate,
                Author = createBlogFromFrontEnd.Author,
                IsVisible = createBlogFromFrontEnd.IsVisible,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in createBlogFromFrontEnd.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(categoryGuid);
                if (existingCategory is not null)
                {
                    blogToCreate.Categories.Add(existingCategory);
                }
            }

            blogToCreate = await _blogPostRepository.CreateAsync(blogToCreate);

            //Convert from domain object to DTO

            var response = new BlogPostDTO()
            {
                Id = blogToCreate.Id,
                Title = blogToCreate.Title,
                ShortDescription = blogToCreate.ShortDescription,
                Content = blogToCreate.Content,
                FeaturedImageUrl = blogToCreate.FeaturedImageUrl,
                UrlHandle = blogToCreate.UrlHandle,
                PublishedDate = blogToCreate.PublishedDate,
                Author = blogToCreate.Author,
                IsVisible = blogToCreate.IsVisible,
                Categories = blogToCreate.Categories.Select(x => new CategoryDTO(x.Id, x.Name, x.UrlHandle)).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> getBlogs()
        {
            var blogPosts = await _blogPostRepository.GetAllBlogsAsync();

            var response = new List<BlogPostDTO>();
            foreach (var blog in blogPosts)
            {
                BlogPostDTO blogDTO = new BlogPostDTO()
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    ShortDescription = blog.ShortDescription,
                    Content = blog.Content,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    UrlHandle = blog.UrlHandle,
                    PublishedDate = blog.PublishedDate,
                    Author = blog.Author,
                    IsVisible = blog.IsVisible,
                };

                foreach (var category in blog.Categories)
                {
                    var categoryDTO = new CategoryDTO(category.Id, category.Name, category.UrlHandle);
                    blogDTO.Categories.Add(categoryDTO);
                }

                response.Add(blogDTO);
            }


            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogFound = await _blogPostRepository.GetBlogByIdAsync(id);

            if (blogFound is null)
            {
                return NotFound();
            }

            BlogPostDTO blogDTOFound = new BlogPostDTO
            {
                Id = blogFound.Id,
                Title = blogFound.Title,
                ShortDescription = blogFound.ShortDescription,
                Content = blogFound.Content,
                FeaturedImageUrl = blogFound.FeaturedImageUrl,
                UrlHandle = blogFound.UrlHandle,
                PublishedDate = blogFound.PublishedDate,
                Author = blogFound.Author,
                IsVisible = blogFound.IsVisible,
                Categories = blogFound.Categories.Select(x => new CategoryDTO(x.Id, x.Name, x.UrlHandle)).ToList()
            };

            return Ok(blogDTOFound);
        }
        
        [HttpGet]
        [Route("{urlHandle}")]

        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPostFound = await _blogPostRepository.GetBlogByUrlHandleAsync(urlHandle);

            if (blogPostFound == null)
            {
                return NotFound();
            }
            
            var response = new BlogPostDTO
            {
                Id = blogPostFound.Id,
                Title = blogPostFound.Title,
                ShortDescription = blogPostFound.ShortDescription,
                Content = blogPostFound.Content,
                FeaturedImageUrl = blogPostFound.FeaturedImageUrl,
                UrlHandle = blogPostFound.UrlHandle,
                PublishedDate = blogPostFound.PublishedDate,
                Author = blogPostFound.Author,
                IsVisible = blogPostFound.IsVisible,
                Categories = blogPostFound.Categories.Select(x => new CategoryDTO(x.Id, x.Name, x.UrlHandle)).ToList()
            };
            return Ok(response);
        }
        
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        //EL [FROM ROUTE] INFORMA QUE EL PARAMETRO DE LA FUNCION VIENE EXPRESAMENTE DE LA RUTA
        // EL [FROM BODY] INFORMA QUE EL PARAMETRO DE LA FUNCION VIENE JUSTAMENTE POR PARAMETRO
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id,
            [FromBody] UpdateBlogPostRequestDTO blogWithUpdates)
        {
            //Tranformé el blogDTO con los cambios hacia un blog, con el fin de poder pasarselo al repositorio
            // El repositorio nunca puede recibir algo que no sea de dominio (Business Logic)

            var updates = new BlogPost
            {
                Id = id,
                Title = blogWithUpdates.Title,
                ShortDescription = blogWithUpdates.ShortDescription,
                Content = blogWithUpdates.Content,
                FeaturedImageUrl = blogWithUpdates.FeaturedImageUrl,
                UrlHandle = blogWithUpdates.UrlHandle,
                PublishedDate = blogWithUpdates.PublishedDate,
                Author = blogWithUpdates.Author,
                IsVisible = blogWithUpdates.IsVisible,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in blogWithUpdates.Categories)
            {

                var category = await _categoryRepository.GetById(categoryGuid);

                if (category != null)
                {
                    updates.Categories.Add(category);
                }
            }
            
            //Consigo el blog actualizado
            var blogUpdated = await _blogPostRepository.UpdateAsync(updates);

            //El blog actualizado lo vuelvo a pasar a DTO, para poder enviarlo al frontend

            if (blogUpdated != null)
            {
                var blogUpdatedDTO = new BlogPostDTO
                {
                    Id = blogUpdated.Id,
                    Title = blogUpdated.Title,
                    ShortDescription = blogUpdated.ShortDescription,
                    Content = blogUpdated.Content,
                    FeaturedImageUrl = blogUpdated.FeaturedImageUrl,
                    UrlHandle = blogUpdated.UrlHandle,
                    PublishedDate = blogUpdated.PublishedDate,
                    Author = blogUpdated.Author,
                    IsVisible = blogUpdated.IsVisible,
                    Categories = blogUpdated.Categories.Select(x => new CategoryDTO(x.Id, x.Name, x.UrlHandle)).ToList()
                };

                return Ok(blogUpdatedDTO);
            }

            return NotFound();
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPostById([FromRoute] Guid id)
        {
            var blogDeleted = await _blogPostRepository.DeleteAsync(id);

            if (blogDeleted != null)
            {
                var blogDeletedDTO = new BlogPostDTO
                {
                    Id = id,
                    Title = blogDeleted.Title,
                    ShortDescription = blogDeleted.ShortDescription,
                    Content = blogDeleted.Content,
                    FeaturedImageUrl = blogDeleted.FeaturedImageUrl,
                    UrlHandle = blogDeleted.UrlHandle,
                    PublishedDate = blogDeleted.PublishedDate,
                    Author = blogDeleted.Author,
                    IsVisible = blogDeleted.IsVisible,
                };
                
                return Ok(blogDeletedDTO);
            }
            return NotFound();
        }
    }
}