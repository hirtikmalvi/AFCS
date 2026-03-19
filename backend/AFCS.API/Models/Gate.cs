namespace AFCS.API.Models
{
    public class Gate
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; } = "";
        public string GateNumber { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
