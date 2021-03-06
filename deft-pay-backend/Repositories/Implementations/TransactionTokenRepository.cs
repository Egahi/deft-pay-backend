﻿using deft_pay_backend.DBContexts;
using deft_pay_backend.Models;
using deft_pay_backend.Repositories.Interfaces;

namespace deft_pay_backend.Repositories.Implementations
{
    public class TransactionTokenRepository : GenericRepository<TransactionToken>, ITransactionTokenRepository
    {
        public TransactionTokenRepository(MariaDbContext context) : base(context, context.TransactionTokens) { }
    }
}
