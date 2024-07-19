using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        #region Constructor
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        #endregion

        #region HttpPost
        //POST {apiBaseUrl}/api/images 
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, 
            [FromForm] string fileName, [FromForm] string title )
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                //File upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };


                blogImage = await _imageRepository.Upload(file, blogImage);

                var response = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                };
                
                return Ok(response);
            }

            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsopported file format");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file","File size cannot be more than 10 MB");
            }

        }
        
        #endregion


        #region HttpGet

        // GET: {apiBaseUrl}/api/Images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAll();
            
            //Convertirlo a DTO

            var response = new List<BlogImageDTO>();
            foreach (var blogImage in images)
            {
                
                response.Add(new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                });
                
            }
            return Ok(response);
        }
        

        #endregion
    }
}
