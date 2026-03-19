using AFCS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFCS.API.Controllers
{
    [Route("api/station")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationService stationService;
        public StationController(IStationService _stationService)
        {
            stationService = _stationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStations()
        {
            var result = await stationService.GetAllStations();
            return Ok(result);
        }
    }
}
