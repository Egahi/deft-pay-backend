using System;

namespace deft_pay_backend.Models
{
    public class RefreshToken
    {
        public string RefreshTokenId { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime GeneratedTime { get; set; }

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
