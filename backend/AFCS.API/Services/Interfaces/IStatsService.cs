using AFCS.API.Common;
using AFCS.API.DTOs.Stats;

namespace AFCS.API.Services.Interfaces
{
    public interface IStatsService
    {
        Task<Result<StatsDTO>> GetSummary();
    }
}
