using Microsoft.AspNetCore.Mvc;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.DTOs;

namespace StockManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        [Route("Products")]
        public async Task<IActionResult> GetProducts()
        {
            _logger.LogInformation("Retrieving list of products");

            var dtos = await _productService.GetAllAsync();

            return Ok(new { Message = "List of products", Data = dtos });
        }
    }
}
