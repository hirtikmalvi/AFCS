using AFCS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFCS.API.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService statsService;
        public StatsController(IStatsService _statsService)
        { 
            statsService = _statsService;
        }
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await statsService.GetSummary();
            return Ok(result);
        }
    }
}
