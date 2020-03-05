using Microsoft.AspNetCore.Identity;
using System;

namespace deft_pay_backend.Repositories.Interfaces
{
    public interface IUserLoginRepository : IGenericRepository<IdentityUserLogin<Guid>>
    {
    }
}
