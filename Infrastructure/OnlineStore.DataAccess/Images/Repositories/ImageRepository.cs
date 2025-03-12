using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Images.Repositories;
using OnlineStore.Contracts.Images;
using OnlineStore.DataAccess.Common;
using OnlineStore.Domain.Entities;


namespace OnlineStore.DataAccess.Images.Repositories
{
    public sealed class ImageRepository : EfRepositoryBase<ProductImage>, IImageRepository
    {
        public ImageRepository(MutableOnlineStoreDbContext mutabledbContext, 
            ReadOnlyOnlineStoreDbContext readOnlydbContext) :
            base(mutabledbContext, readOnlydbContext)
        {


        }

        /// <inheritdoc/>
        public Task<ProductImage?> GetByUrlAsync(string url, CancellationToken cancellation)
        {
            return _mutableDbContext.Set<ProductImage>().FirstOrDefaultAsync(i => i.Url == url, cancellation);
        }

        /// <inheritdoc/>
        public async Task<int> SaveAsync(ProductImage image, CancellationToken cancellation)
        {
            await _mutableDbContext.AddAsync(image, cancellation);
            await _mutableDbContext.SaveChangesAsync(cancellation);

            return image.Id;
        }

        /// <inheritdoc/>
        public async Task<int> ChangeAsync(ProductImage image, CancellationToken cancellation)
        {
            await _mutableDbContext.AddAsync(image, cancellation);
            await _mutableDbContext.SaveChangesAsync(cancellation);

            return image.Id;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(ProductImage image)
        {
             _mutableDbContext.Remove(image);
            return _mutableDbContext.SaveChangesAsync(); 
        }

    }
}
