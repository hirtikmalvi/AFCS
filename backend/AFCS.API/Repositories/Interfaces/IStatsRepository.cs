using AFCS.API.DTOs.Stats;

namespace AFCS.API.Repositories.Interfaces
{
    public interface IStatsRepository
    {
        Task<StatsDTO> GetSummary(); 
    }
}
