using System.ComponentModel.DataAnnotations;

namespace deft_pay_backend.ModelsDTO.Requests
{
    public class RefreshTokenDTO
    {
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
