using AFCS.API.Common;
using AFCS.API.DTOs.Station;
using AFCS.API.Models;

namespace AFCS.API.Services.Interfaces
{
    public interface IStationService
    {
        Task<Result<List<StationDTO>>> GetAllStations();
    }
}
