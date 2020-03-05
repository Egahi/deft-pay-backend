using Microsoft.AspNetCore.Identity;
using System;

namespace deft_pay_backend.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() : base()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string BVN { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Email { get; set; }
        public string Phonenumber1 { get; set; }
        public string Phonenumber2 { get; set; }
        public string EnrollmentBank { get; set; }
        public bool ShouldDelete { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
        public DateTime? TimeDeleted { get; set; }
        public DateTime? TimeDeleteRequestReceived { get; set; }
    }
}
