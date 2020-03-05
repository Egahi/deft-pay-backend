using deft_pay_backend.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace deft_pay_backend.ModelsDTO.Requests
{
    public class UserRegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "{0} must be atleast 2 characters long.")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "{0} can only contain alphabets")]
        public string FirstName { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 2, ErrorMessage = "{0} must be atleast 2 characters long.")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "{0} can only contain alphabets")]
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Url]
        public string ProfilePictureUrl { get; set; }
        [StringRange(AllowableValues = new[] { "Male", "Female", null }, ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }
        [Range(typeof(DateTime), "01/01/1820", "01/01/2820", ErrorMessage = "Value for {0} must be between {1} and {2}")]
        [RegularExpression("^([012]\\d|30|31)/(0\\d|10|11|12)/\\d{4}$", ErrorMessage = "Date must be in the format dd/mm/yyyy e.g 22/12/2020")]
        public string DateOfBirth { get; set; }
    }
}
