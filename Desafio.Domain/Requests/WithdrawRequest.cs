using System;

namespace Desafio.Domain.Requests
{
    public class WithdrawRequest
    {
        public Guid AccountId { get; set; }
        public decimal Value { get; set; }
    }
}
