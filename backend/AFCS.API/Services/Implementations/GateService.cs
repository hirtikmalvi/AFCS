using AFCS.API.Common;
using AFCS.API.DTOs.Gate;
using AFCS.API.Hubs;
using AFCS.API.Repositories.Interfaces;
using AFCS.API.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AFCS.API.Services.Implementations
{
    public class GateService: IGateService
    {
        private readonly IGateRepository gateRepository;
        private readonly IHubContext<FareHub> hub;

        public GateService(IGateRepository _gateRepository, IHubContext<FareHub> _hub)
        {
            gateRepository = _gateRepository;
            hub = _hub;
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

            // Broadcast gate change to all dashboards
            await hub.Clients.All.SendAsync(FareHub.GateStatusChanged, gateDTO);

            return Result<GateDTO>.Ok(gateDTO);
        }
    }
}
