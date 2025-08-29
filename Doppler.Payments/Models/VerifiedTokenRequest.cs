// unset:error

namespace Doppler.Payments.Models
{
    public class VerifiedTokenRequest
    {
        public string TokenId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
    }
}
