using System;

namespace Desafio.Domain.Responses
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}
