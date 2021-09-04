using System;

namespace Desafio.Domain.Responses
{
    public class TransferResponse
    {
        public Guid OriginAccountId { get; set; }
        public string OriginAccountName { get; set; }
        public decimal OriginAccountNewBalance { get; set; }
        public Guid DestinationAccountId { get; set; }
        public string DestinationAccountName { get; set; }
        public decimal DestinationAccountNewBalance { get; set; }
    }
}
