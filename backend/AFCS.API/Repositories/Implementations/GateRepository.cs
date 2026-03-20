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

        public async Task<GateDTO> GetGateById(int gateId)
        {
            await using var conn = new NpgsqlConnection(dbConnString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM get_gate_by_id_with_station_name(@p_id)";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@p_id", gateId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return getGateDTO(reader);
            }
            return null!;
        }

        public async Task<bool> UpdateStatus(int gateId, string status)
        {
            await using var conn = new NpgsqlConnection(dbConnString);

            await conn.OpenAsync();

            var sql = "UPDATE gates SET status = @status WHERE id = @id";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", gateId);

            var reader = await cmd.ExecuteNonQueryAsync();

            return reader > 0;
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
