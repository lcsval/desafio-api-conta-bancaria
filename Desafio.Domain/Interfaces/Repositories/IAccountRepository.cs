using Desafio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desafio.Domain.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAll();
        Task<Account> GetById(Guid id);
        Task<Account> Create(Account account);
        Task<Account> Update(Account account);
    }
}
