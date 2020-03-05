using deft_pay_backend.DBContexts;
using deft_pay_backend.Models;
using deft_pay_backend.Repositories.Implementations;
using deft_pay_backend.Repositories.Interfaces;

namespace deft_pay_backend.ModelsDTO.Responses
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(MariaDbContext context) : base(context, context.RefreshTokens) { }
    }
}
