using System;

namespace Desafio.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Balance { get; set; }
    }
}
