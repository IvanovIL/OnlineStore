using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.MVC.Attributes;


namespace OnlineStore.MVC.WebApi
{
    
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [Route("add/product")]
        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromBody] ShortProductDto productDto, CancellationToken cancellation)
        {
            await _productsService.AddProductAsync(productDto, cancellation);

            return NoContent();
        }



        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetProductAsync(CancellationToken cancellation)
        {
            var result = await _productsService.GetProductsAsync(new PagedRequest
            {
                PageNumber = 1,
                PageSize = 10
            }, cancellation);

            return Ok(result);
        }

    }
}
