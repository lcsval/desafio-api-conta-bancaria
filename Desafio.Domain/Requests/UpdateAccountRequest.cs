using System;

namespace Desafio.Domain.Requests.Customer
{
    public class UpdateAccountRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}
