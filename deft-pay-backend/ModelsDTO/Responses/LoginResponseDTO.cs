using System;
using System.Collections.Generic;

namespace deft_pay_backend.ModelsDTO.Responses
{
    public class LoginResponseDTO
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiryTime { get; set; }
        public ICollection<string> Roles { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
    }
}
