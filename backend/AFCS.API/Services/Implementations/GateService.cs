using AFCS.API.Common;
using AFCS.API.DTOs.Gate;
using AFCS.API.Repositories.Interfaces;
using AFCS.API.Services.Interfaces;

namespace AFCS.API.Services.Implementations
{
    public class GateService: IGateService
    {
        private readonly IGateRepository gateRepository;
        public GateService(IGateRepository _gateRepository)
        {
            gateRepository = _gateRepository;
        }

        public async Task<Result<List<GateDTO>>> GetAllGates()
        {
            var gates = await gateRepository.GetAllGates();
            return Result<List<GateDTO>>.Ok(gates);
        }

        public async Task<Result<GateDTO>> UpdateStatus(int gateId, string status)
        {
            var gate = await gateRepository.GetGateById(gateId);

            if (gate == null)
            {
                return Result<GateDTO>.NotFound([$"Gate with id ${gateId} not found."]);
            }

            await gateRepository.UpdateStatus(gateId, status);
            
            gate.Status = status.ToLower();

            var gateDTO = new GateDTO
            {
                Id = gate.Id,
                StationId = gate.StationId,
                StationName = gate.StationName,
                GateNumber = gate.GateNumber,
                Status = status,
            };

            return Result<GateDTO>.Ok(gateDTO);
        }
    }
}
