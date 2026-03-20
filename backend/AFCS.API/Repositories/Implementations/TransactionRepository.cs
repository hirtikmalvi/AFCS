using AFCS.API.DTOs.Transaction;
using AFCS.API.Repositories.Interfaces;
using Npgsql;

namespace AFCS.API.Repositories.Implementations
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string dbConnString;
        private readonly IConfiguration configuration;

        public TransactionRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            dbConnString = configuration.GetConnectionString("DbConnectionString")!;
        }

        public async Task<List<TransactionDTO>> GetRecentTransactions(int limit = 20)
        {
            var transactions = new List<TransactionDTO>();

            await using var conn = new NpgsqlConnection(dbConnString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM get_all_transactions_with_gatenumber_and_stationame(@recent)";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@recent", limit);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                transactions.Add(getTransactionDTO(reader));
            }

            return transactions;
        }

        private static TransactionDTO getTransactionDTO(NpgsqlDataReader r)
        {
            var transaction = new TransactionDTO
            {
                Id = r.GetInt32(r.GetOrdinal("id")),
                GateId = r.GetInt32(r.GetOrdinal("gate_id")),
                GateNumber = r.GetString(r.GetOrdinal("gate_number")),
                StationName = r.GetString(r.GetOrdinal("station_name")),
                CardNumber = r.GetString(r.GetOrdinal("card_number")),
                FareAmount = r.GetDecimal(r.GetOrdinal("fare_amount")),
                TransactionTime = r.GetDateTime(r.GetOrdinal("transaction_time")),
                PaymentType = r.GetString(r.GetOrdinal("payment_type"))
            };
            return transaction;
        }
    }
}
