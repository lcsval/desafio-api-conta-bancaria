using Desafio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desafio.Domain.Interfaces.Repositories
{
    public interface IAccountRecordRepository
    {
        Task<List<AccountRecord>> GetAll(Guid accountId);
        Task<AccountRecord> GetById(Guid id);
        Task<AccountRecord> Create(AccountRecord accRecord);
    }
}
