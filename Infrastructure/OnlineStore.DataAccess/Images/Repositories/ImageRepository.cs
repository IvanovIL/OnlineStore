using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Images.Repositories;
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

        public Task<ProductImage?> GetByUrlAsync(string url, CancellationToken cancellation)
        {
            return _mutableDbContext.Set<ProductImage>().FirstOrDefaultAsync(i => i.Url == url, cancellation);
        }

        public async Task<int> SaveAsync(ProductImage image, CancellationToken cancellation)
        {
            await _mutableDbContext.AddAsync(image, cancellation);
            await _mutableDbContext.SaveChangesAsync(cancellation);

            return image.Id;
        }
    }
}
