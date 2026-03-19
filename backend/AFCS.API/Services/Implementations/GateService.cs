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
    }
}
