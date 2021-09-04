using System;

namespace Desafio.Domain.Responses
{
    public class DepositResponse
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public decimal NewBalance { get; set; }
    }
}
