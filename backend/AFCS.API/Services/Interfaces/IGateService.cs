using AFCS.API.Common;
using AFCS.API.DTOs.Gate;

namespace AFCS.API.Services.Interfaces
{
    public interface IGateService
    {
        Task<Result<List<GateDTO>>> GetAllGates();
        Task<Result<GateDTO>> UpdateStatus(int gateId, string status);
    }
}
