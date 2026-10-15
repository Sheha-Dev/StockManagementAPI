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

        [HttpGet]
        [Route("Branches")]
        public IActionResult GetBranches()
        {
            _logger.LogInformation("Retrieving list of branches");

            var response = _branchService.GetAllAsync(); 
            // Logic to retrieve branches from the database
            return Ok(new { Message = "List of branches", Data = response });
        }
    }
}
