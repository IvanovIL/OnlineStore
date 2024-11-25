using Microsoft.AspNetCore.Http;
using OnlineStore.Contracts.Images;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Images.Services
{
    public interface  IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, CancellationToken cancellation);

        Task<ImageDto> GetImageDtoAsync(int id, CancellationToken cancellation);

        string[] GetImagesUrls(ProductImage[] images);

        Task<IReadOnlyCollection<string>> SaveImagesAsync(List<IFormFile> ImageFiles, CancellationToken cancellation);

        Task<ProductImage[]> SaveProductImagesAsync(string[] imagesUrls, Product product, CancellationToken cancellation);
    }
}
