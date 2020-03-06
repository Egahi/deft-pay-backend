using System.ComponentModel.DataAnnotations;

namespace deft_pay_backend.ModelsDTO.Requests
{
    public class LoginRequestDTO
    {
        public string BVN { get; set; }
        [Url]
        public string ProfilePicture { get; set; }
    }
}
