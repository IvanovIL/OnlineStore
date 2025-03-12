using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Contracts.Images;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Images.Services
{
    /// <summary>
    /// Интерфейс сервиса по работе с изображением продукта
    /// </summary>
    public interface  IImageService
    {
        /// <summary>
        /// Сохраняет изображение в БД
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="cancellation"></param>
        Task<string> SaveImageAsync(IFormFile imageFile, CancellationToken cancellation);

        /// <summary>
        /// Получает изображение по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<ImageDto> GetImageDtoAsync(int id, CancellationToken cancellation);

        /// <summary>
        /// Получает изображение
        /// </summary>
        /// <param name="images"></param>
        string[] GetImagesUrls(ProductImage[] images);

        /// <summary>
        /// Получает изображение для сохранения в БД и создает массив
        /// </summary>
        /// <param name="ImageFiles">Список файлов изображений</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<IReadOnlyCollection<string>> SaveImagesAsync(List<IFormFile> ImageFiles, CancellationToken cancellation);

        /// <summary>
        /// Созраняет и привязывает изображение к продукту
        /// </summary>
        /// <param name="imagesUrls">Url изображения</param>
        /// <param name="product">Продукт</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<ProductImage[]> SaveProductImagesAsync(string[] imagesUrls, Product product, CancellationToken cancellation);

        /// <summary>
        /// Удаляет изображение из БД
        /// </summary>
        /// <param name="imageUrl">Url изображения</param>
        Task DeleteImageAsync([FromBody] ImageDeleteRequest imageUrl);
    }
}
