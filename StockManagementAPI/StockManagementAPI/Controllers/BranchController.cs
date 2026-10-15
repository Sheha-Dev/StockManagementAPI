using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManagementAPI.Interfaces;

namespace StockManagementAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IBranchService _branchService;
        public BranchController(ILogger<BranchController> logger, IBranchService branchService)
        {
            _logger = logger;
            _branchService = branchService;
        }

        [HttpGet("Branches")]
        public async Task<IActionResult> GetBranches()
        {
            _logger.LogInformation("Retrieving list of branches");

            var response = await _branchService.GetAllAsync();

            return Ok(new
            {
                Message = "List of branches",
                Data = response
            });
        }
    }
}
