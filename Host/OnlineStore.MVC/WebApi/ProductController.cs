using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Domain.Entities;
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

        [Route("Add/product")]
        [HttpPost]
        public async Task<IActionResult> AddProductsAsync(CancellationToken cancellation)
        {
            return Ok();
        }

    }
}
