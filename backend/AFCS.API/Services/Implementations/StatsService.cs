using AFCS.API.Common;
using AFCS.API.DTOs.Stats;
using AFCS.API.Repositories.Interfaces;
using AFCS.API.Services.Interfaces;

namespace AFCS.API.Services.Implementations
{
    public class StatsService: IStatsService
    {
        private readonly IStatsRepository statsRepository;
        public StatsService(IStatsRepository _statsRepository)
        {
            statsRepository = _statsRepository;
        }

        public async Task<Result<StatsDTO>> GetSummary()
        {
            var summary = await statsRepository.GetSummary();
            return Result<StatsDTO>.Ok(summary);
        }
    }
}
