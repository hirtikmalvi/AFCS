using AFCS.API.DTOs.Gate;
using AFCS.API.Repositories.Interfaces;
using Npgsql;

namespace AFCS.API.Repositories.Implementations
{
    public class GateRepository : IGateRepository
    {
        private readonly string dbConnString;
        private readonly IConfiguration configuration;
        public GateRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            dbConnString = configuration.GetConnectionString("DbConnectionString")!;
        }
        public async Task<List<GateDTO>> GetAllGates()
        {
            var gates = new List<GateDTO>();
            await using var conn = new NpgsqlConnection(dbConnString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM get_all_gates_with_station_name()";

            await using var cmd = new NpgsqlCommand(sql, conn);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                gates.Add(getGateDTO(reader));
            }
            return gates;
        }

        private GateDTO getGateDTO(NpgsqlDataReader r)
        {
            var gate = new GateDTO
            {
                Id = r.GetInt32(r.GetOrdinal("id")),
                StationId = r.GetInt32(r.GetOrdinal("stationid")),
                StationName = r.GetString(r.GetOrdinal("stationname")),
                GateNumber = r.GetString(r.GetOrdinal("gatenumber")),
                Status = r.GetString(r.GetOrdinal("status"))
            };
            return gate;
        }
    }
}
