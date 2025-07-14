using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Interface;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController(IProjectRepository reportService) : ControllerBase
    {
        private readonly IProjectRepository _reportService = reportService;

        [HttpGet("performance")]
        public async Task<IActionResult> GetPerformanceReport()
        {
            var userId = GetUserId();
            var report = await _reportService.GetByIdAsync(userId);

            var json = System.Text.Json.JsonSerializer.Serialize(report, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            });

            return Ok(json);
        }

        private int GetUserId()
        {
            return Request.Headers.ContainsKey("UserId") ?
                int.Parse(Request.Headers["UserId"]!) : 1;
        }
    }
}
