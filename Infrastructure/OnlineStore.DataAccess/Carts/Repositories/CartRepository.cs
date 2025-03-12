using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Carts.Repositories;
using OnlineStore.Contracts.Enums;
using OnlineStore.DataAccess.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.DataAccess.Carts.Repositories
{
    /// <summary>
    /// Репозиторий корзины
    /// </summary>
    public sealed class CartRepository : EfRepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(
            MutableOnlineStoreDbContext mutableDbContext,
            ReadOnlyOnlineStoreDbContext readOnlyDbContext)
            : base(mutableDbContext, readOnlyDbContext)
        {

        }

        /// <inheritdoc/>
        public Task<Cart> GetCartByUserAsync(int userId, CancellationToken cancellation)
        {
            return _mutableDbContext.Set<Cart>()
                .Where(c => c.UserId == userId)
                .Where(c => c.StatusId != (int)CartStatusEnum.Done)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(cancellation);
        }


        /// <inheritdoc/>
        public Task<CartProduct> GetCartItemId(int id, int userId, CancellationToken cancellation)
        {
            return _mutableDbContext.Set<CartProduct>()
                .Where (c => c.CartId == userId)
                .Where(c => c.ProductId == id)
               .FirstOrDefaultAsync(cancellation);
        }
    }
}
