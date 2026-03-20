using AFCS.API.DTOs.Gate;

namespace AFCS.API.Repositories.Interfaces
{
    public interface IGateRepository
    {
        Task<List<GateDTO>> GetAllGates();
        Task<GateDTO> GetGateById(int gateId);
        Task<bool> UpdateStatus(int gateId, string status);
    }
}
