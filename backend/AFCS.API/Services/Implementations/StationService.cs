using AFCS.API.Common;
using AFCS.API.DTOs.Station;
using AFCS.API.Models;
using AFCS.API.Repositories.Interfaces;
using AFCS.API.Services.Interfaces;

namespace AFCS.API.Services.Implementations
{
    public class StationService : IStationService
    {
        private readonly IStationRepository stationRepository;

        public StationService(IStationRepository _stationRepository)
        {
            stationRepository = _stationRepository;
        }

        public async Task<Result<List<StationDTO>>> GetAllStations()
        {
            var stations = await stationRepository.GetAllStations();

            return Result<List<StationDTO>>.Ok(stations);
        }
    }
}
