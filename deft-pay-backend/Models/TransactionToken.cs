using deft_pay_backend.Utilities;
using System;

namespace deft_pay_backend.Models
{
    public class TransactionToken
    {
        public Guid TransactionTokenId { get; set; } = Guid.NewGuid();
        public double Amount { get; set; } = 0.00;
        public string OTP { get; set; } = Helper.GetRandomToken(30);
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddHours(3);
        public bool IsUsed { get; set; }
        public bool IsExpired { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}
