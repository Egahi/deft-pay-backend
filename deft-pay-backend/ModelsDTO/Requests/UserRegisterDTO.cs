using System.ComponentModel.DataAnnotations;

namespace deft_pay_backend.ModelsDTO.Requests
{
    public class UserRegisterDTO
    {
        [Required]
        public string BVN { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "{0} must be atleast 2 characters long.")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "{0} can only contain alphabets")]
        public string FirstName { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "{0} must be atleast 2 characters long.")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "{0} can only contain alphabets")]
        public string MiddleName { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "{0} must be atleast 2 characters long.")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "{0} can only contain alphabets")]
        public string LastName { get; set; }

        public string DateOfBirth { get; set; }
        [Phone]
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        [Url]
        public string ProfilePictureUrl { get; set; }
        [Required]
        public string EnrollmentBank { get; set; }
        public string Email { get; set; }
    }
}
