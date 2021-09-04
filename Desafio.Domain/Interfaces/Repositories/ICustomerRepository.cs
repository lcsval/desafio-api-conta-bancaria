using Desafio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desafio.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAll();
        Task<Customer> GetById(Guid id);
        Task<Customer> Create(Customer customer);
        Task<Customer> Update(Customer customer);
        Task Delete(Guid id);
    }
}
