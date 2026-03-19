namespace AFCS.API.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int GateId { get; set; }
        public string GateNumber { get; set; } = "";
        public string StationName { get; set; } = ""; 
        public string CardNumber { get; set; } = "";
        public decimal FareAmount { get; set; }
        public DateTime TransactionTime { get; set; }
        public string PaymentType { get; set; } = "";
    }
}
