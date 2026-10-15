using Microsoft.AspNetCore.Mvc;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.DTOs;

namespace StockManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestStatusController : ControllerBase
    {
        private readonly ILogger<RequestStatusController> _logger;
        private readonly IRequestStatusService _service;

        public RequestStatusController(ILogger<RequestStatusController> logger, IRequestStatusService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("RequestStatuses")]
        public async Task<IActionResult> GetRequestStatuses()
        {
            _logger.LogInformation("Retrieving list of request statuses");

            var dtos = await _service.GetAllAsync();

            return Ok(new { Message = "List of request statuses", Data = dtos });
        }
    }
}
