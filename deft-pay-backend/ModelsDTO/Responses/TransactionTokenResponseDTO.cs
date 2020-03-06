using System;

namespace deft_pay_backend.ModelsDTO.Responses
{
    public class TransactionTokenResponseDTO
    {
        public string OTP { get; set; }
        public string ExpiryDate { get; set; }
        public string ExpiryTime { get; set; }

        public UserProfileSummaryDTO User { get; set; }
    }
}
