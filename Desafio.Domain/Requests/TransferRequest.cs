using System;

namespace Desafio.Domain.Requests
{
    public class TransferRequest
    {
        public Guid OriginAccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public decimal Value { get; set; }
    }
}
