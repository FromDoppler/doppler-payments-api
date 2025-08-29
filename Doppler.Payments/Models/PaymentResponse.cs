// unset:error

using System;

namespace Doppler.Payments.Models
{
    public class PaymentResponse
    {
        public string Id { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
