using System;

namespace Desafio.Domain.Responses
{
    public class AccountRecordResponse
    {
        public DateTime Date { get; set; }
        public string Operation { get; set; }
        public decimal Value { get; set; }
        public string Type { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalValue { get; set; }
    }
}
