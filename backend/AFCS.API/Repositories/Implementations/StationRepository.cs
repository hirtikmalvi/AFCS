using AFCS.API.DTOs.Station;
using AFCS.API.Models;
using AFCS.API.Repositories.Interfaces;
using Npgsql;

namespace AFCS.API.Repositories.Implementations
{
    public class StationRepository : IStationRepository
    {
        private readonly string dbConnString;
        private readonly IConfiguration configuration;

        public StationRepository(IConfiguration _configuration) 
        { 
            configuration = _configuration;
            dbConnString = configuration.GetConnectionString("DbConnectionString")!;
        }
        public async Task<List<StationDTO>> GetAllStations()
        {
            var stations = new List<StationDTO>();
            
            await using var conn = new NpgsqlConnection(dbConnString);

            await conn.OpenAsync();

            var sql = "SELECT id, name, location, is_active FROM stations ORDER BY name";

            await using var cmd = new NpgsqlCommand(sql,conn);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var station = new StationDTO
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Location = reader.GetString(reader.GetOrdinal("location")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active"))
                };
                stations.Add(station);
            }
            return stations;
        }
    }
}
