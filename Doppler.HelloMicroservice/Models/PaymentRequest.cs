// unset:error

namespace Doppler.HelloMicroservice.Models
{
    public class PaymentRequest
    {
        public string TokenId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string TransactionReference { get; set; } = null!;
    }
}
