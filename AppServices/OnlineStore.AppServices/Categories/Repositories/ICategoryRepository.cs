using OnlineStore.AppServices.Common;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Categories.Repositories
{
    /// <summary>
    /// Интерфейс репозитория категории продуктов
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>;
}
