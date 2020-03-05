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
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool ShouldDelete { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
        public DateTime? TimeDeleted { get; set; }
        public DateTime? TimeDeleteRequestReceived { get; set; }
    }
}
