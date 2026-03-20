using System.ComponentModel.DataAnnotations;

namespace AFCS.API.DTOs.Transaction
{
    public class CreateTransactionRequestDTO
    {
        [Required(ErrorMessage = "GateId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "GateId is required.")]
        public int? GateId { get; set; }
        [Required(ErrorMessage = "CardNumber is required.")]
        public string? CardNumber { get; set; } = "";
        [Required (ErrorMessage = "FareAmount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "FairAmount must be alteast 0.")]
        public decimal? FareAmount { get; set; }
        [Required(ErrorMessage = "PaymentType is required.")]
        [AllowedValues(["card", "cash", "qr"], ErrorMessage ="Only card, cash and qr PaymentType is allowed.")]
        public string? PaymentType { get; set; } = "";
    }
}
