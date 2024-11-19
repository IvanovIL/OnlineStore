

namespace OnlineStore.Contracts.Product
{
    public sealed class ShortProductList
    {
        public ShortProductDto[] Products { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }
    }
}
