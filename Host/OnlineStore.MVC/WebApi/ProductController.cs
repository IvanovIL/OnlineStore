using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.MVC.Attributes;


namespace OnlineStore.MVC.WebApi
{
   /// <summary>
   /// Контроллер по управлению продуктами
   /// </summary>

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

        [Route("delete/product")]
        [HttpPost]
        public async Task<IActionResult> DeleteProductAsync(int idIsDeleted, CancellationToken cancellation)
        {
            var result = _productsService.DeleteProductAsync(idIsDeleted, cancellation);

            return Ok(result);
        }

        [Route("change/product")]
        [HttpPost]
        public async Task<IActionResult> ChangeProductAsync([FromBody] ShortProductDto productDto, CancellationToken cancellation)
        {
            var result = _productsService.ChangeProductAsync(productDto, cancellation);

            return Ok(result);
        }


        [Route("find/product")]
        [HttpPost]
        public async Task<IActionResult> FindProductAsync([FromBody] ShortProductDto productDto, CancellationToken cancellation)
        {
            var result = await _productsService.FindProductAsync(productDto, new PagedRequest
            {
                PageNumber = 1,
                PageSize = 10
            }, cancellation);

            return Ok(result);
        }
    }
}
