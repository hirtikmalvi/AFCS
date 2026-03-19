using AFCS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AFCS.API.Controllers
{
    [Route("api/gate")]
    [ApiController]
    public class GateController : ControllerBase
    {
        private readonly IGateService gateService;
        public GateController(IGateService _gateService)
        { 
            gateService = _gateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGates()
        {
            var result = await gateService.GetAllGates();
            return Ok(result);
        }
    }
}
