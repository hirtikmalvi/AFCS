using AFCS.API.Common;
using AFCS.API.DTOs.Gate;
using AFCS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpPost("{gateId}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int gateId, [FromBody] UpdateGateStatusRequest request)
        {
            if (gateId != request.GateId)
            {
                return Ok(Result<GateDTO>.BadRequest(["GateId mismatch."]));
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0).SelectMany(kvp => kvp.Value.Errors).Select(e => e.ErrorMessage).ToList();
                return Ok(Result<GateDTO>.BadRequest(errors));
            }

            var result = await gateService.UpdateStatus((int)request.GateId!, request.Status!);

            return Ok(result);
        }
    }
}
