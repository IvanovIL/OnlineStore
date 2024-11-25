

namespace OnlineStore.Contracts.Product
{
    public sealed class ShortProductList
    {
        public  ShortProductDto[] Product { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }
    }
}
