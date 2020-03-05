using deft_pay_backend.DBContexts;
using deft_pay_backend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace deft_pay_backend.Repositories.Implementations
{
    public class UserLoginRepository : GenericRepository<IdentityUserLogin<Guid>>, IUserLoginRepository
    {
        public UserLoginRepository(MariaDbContext context) : base(context, context.UserLogins) { }
    }
}
