namespace AFCS.API.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
