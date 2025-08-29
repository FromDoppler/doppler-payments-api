// unset:error

namespace Doppler.Payments.Models
{
    public class TokenRequest
    {
        public string CardNumber { get; set; } = null!;
        public string ExpiryMonth { get; set; } = null!;
        public string ExpiryYear { get; set; } = null!;
        public string Cvv { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
