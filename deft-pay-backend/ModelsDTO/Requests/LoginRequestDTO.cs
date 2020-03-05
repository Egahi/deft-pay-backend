using System.ComponentModel.DataAnnotations;

namespace deft_pay_backend.ModelsDTO.Requests
{
    public class LoginRequestDTO
    {
        [Required]
        [Url]
        public string ProfilePicture { get; set; }
    }
}
