using Microsoft.AspNetCore.Mvc;
using StockManagementAPI.Interfaces;
using StockManagementAPI.Models.DTOs;

namespace StockManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly ILogger<RequestController> _logger;
        private readonly IRequestService _service;
        private readonly IRequestRepository _repo;

        public RequestController(ILogger<RequestController> logger, IRequestService service, IRequestRepository repo)
        {
            _logger = logger;
            _service = service;
            _repo = repo;
        }

        [HttpGet]
        [Route("Requests")]
        public async Task<IActionResult> GetRequests([
            FromQuery] Guid? requestStatusId,
            [FromQuery] Guid? fromBranchId,
            [FromQuery] Guid? toBranchId,
            [FromQuery] Guid? productId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Retrieving list of requests");

            var dtos = await _service.GetAllAsync(requestStatusId, fromBranchId, toBranchId, productId, pageNumber, pageSize);

            return Ok(new { Message = "List of requests", Data = dtos });
        }

        [HttpPost]
        [Route("Requests")]
        public async Task<IActionResult> CreateRequest([FromBody] StockManagementAPI.Models.DTOs.StockRequestDto dto)
        {
            _logger.LogInformation("Creating new request");

            // createdBy should come from authenticated user; using a placeholder for now
            var createdBy = User?.Identity?.Name ?? "system";

            await _service.AddAsync(dto, createdBy);

            return CreatedAtAction(nameof(GetRequests), null);
        }

        [HttpPost("{id:guid}/dispatch")]
        public async Task<IActionResult> Dispatch([FromRoute] Guid id)
        {
            var user = User?.Identity?.Name ?? "system";
            await _repo.DispatchAsync(id, user);
            return NoContent();
        }

        [HttpPost("{id:guid}/receive")]
        public async Task<IActionResult> Receive([FromRoute] Guid id)
        {
            var user = User?.Identity?.Name ?? "system";
            await _repo.ReceiveAsync(id, user);
            return NoContent();
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] Guid id)
        {
            var user = User?.Identity?.Name ?? "system";
            await _repo.CancelAsync(id, user);
            return NoContent();
        }

        
    }
}
