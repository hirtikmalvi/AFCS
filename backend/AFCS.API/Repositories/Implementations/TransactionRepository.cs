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

        public async Task<TransactionDTO> CreateTransaction(CreateTransactionRequestDTO request)
        {
            await using var conn = new NpgsqlConnection(dbConnString);
            await conn.OpenAsync();

            var sql = "SELECT * FROM create_transaction(@p_gate_id, @p_card_number, @p_fare_amount, @p_payment_type)";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@p_gate_id", (object)request.GateId!);
            cmd.Parameters.AddWithValue("@p_card_number", request.CardNumber!);
            cmd.Parameters.AddWithValue("@p_fare_amount", request.FareAmount!);
            cmd.Parameters.AddWithValue("@p_payment_type", request.PaymentType!);

            await using var reader = await cmd.ExecuteReaderAsync();

            await reader.ReadAsync();

            var transaction = new TransactionDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("new_id")),
                TransactionTime = reader.GetDateTime(reader.GetOrdinal("new_transaction_time")),
                GateId = (int)request.GateId!,
                CardNumber = string.Concat(request.CardNumber!.Substring(0, 4), "****"),
                FareAmount = (decimal)request.FareAmount!,
                PaymentType = request
                .PaymentType!
            };
            return transaction;
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
