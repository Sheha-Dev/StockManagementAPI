using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManagementAPI.Interfaces;

namespace StockManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchStockController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public BranchStockController(ILogger<ProductController> logger, IProductService productService)
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
