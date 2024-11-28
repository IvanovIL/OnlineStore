using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Carts.Repositories;
using OnlineStore.Contracts.Enums;
using OnlineStore.DataAccess.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Carts.Repositories
{
    public sealed class CartRepository : EfRepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(
            MutableOnlineStoreDbContext mutableDbContext,
            ReadOnlyOnlineStoreDbContext readOnlyDbContext)
            : base(mutableDbContext, readOnlyDbContext)
        {

        }

        public Task<Cart> GetCartByUserAsync(int userId, CancellationToken cancellation)
        {
            return _mutableDbContext.Set<Cart>()
                .Where(c => c.UserId == userId)
                .Where(c => c.StatusId == (int)CartStatusEnum.New)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(cancellation);
        }
    }
}
