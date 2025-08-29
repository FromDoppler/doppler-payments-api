// unset:error

namespace Doppler.Payments.Models
{
    public class TokenResponse
    {
        public string Id { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Last4 { get; set; } = null!;
        public string ExpiryMonth { get; set; } = null!;
        public string ExpiryYear { get; set; } = null!;
    }
}
