using AFCS.API.DTOs.Station;
using AFCS.API.Models;

namespace AFCS.API.Repositories.Interfaces
{
    public interface IStationRepository
    {
        Task<List<StationDTO>> GetAllStations();
    }
}
