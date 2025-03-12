using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Images.Services;
using OnlineStore.Contracts.Images;

namespace OnlineStore.MVC.Controllers
{
    /// <summary>
    /// Контроллер по работе с изображением
    /// </summary>
    public class ImagesController : Controller
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("images/{imageId}")]
        public async Task<IActionResult> Get([FromRoute] int imageId, CancellationToken cancellation)
        {
            var image = await _imageService.GetImageDtoAsync(imageId, cancellation);

            return File(image.Data, image.ContentType);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> ImageFiles, CancellationToken cancellation)
        {
            var imageUrls = await _imageService.SaveImagesAsync(ImageFiles, cancellation);

            return Json(imageUrls);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUploadImages(List<IFormFile> ImageFiles,CancellationToken cancellation)
        { 
            var imageUrls = await _imageService.SaveImagesAsync(ImageFiles, cancellation);

            return Json(imageUrls);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage([FromBody] ImageDeleteRequest imageUrl)
        {
            await _imageService.DeleteImageAsync(imageUrl);
            return Json(null);
        }
    }
}
