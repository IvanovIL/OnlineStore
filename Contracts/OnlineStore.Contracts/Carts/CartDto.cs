
namespace OnlineStore.Contracts.Cart
{
    public sealed class CartDto
    {
        public List<CartItemDto> Items { get; set; } = [];

        public decimal TotalAmount { get; set; }

    }
}
