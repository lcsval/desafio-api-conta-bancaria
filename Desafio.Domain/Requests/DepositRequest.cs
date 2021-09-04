using System;

namespace Desafio.Domain.Requests
{
    public class DepositRequest
    {
        public Guid AccountId { get; set; }
        public decimal Value { get; set; }
    }
}
