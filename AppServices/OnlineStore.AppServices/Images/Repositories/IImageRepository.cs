using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Common;
using OnlineStore.Contracts.Images;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Images.Repositories
{
    public interface IImageRepository : IRepository<ProductImage>
    {
        Task<int> SaveAsync(ProductImage image, CancellationToken cancellation);

        Task<ProductImage?> GetByUrlAsync(string url, CancellationToken cancellation);


        Task<int> ChangeAsync(ProductImage image, CancellationToken cancellation);

        Task DeleteAsync(ProductImage image);
    }
}
