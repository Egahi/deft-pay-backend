using System.ComponentModel.DataAnnotations;

namespace deft_pay_backend.ModelsDTO.Requests
{
    public class LoginRequestDTO
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
