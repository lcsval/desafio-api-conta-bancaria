using System;
using System.Collections.Generic;

namespace Desafio.Domain.Responses
{
    public class ExtractResponse
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public List<AccountRecordResponse> Records { get; set; }
    }
}
