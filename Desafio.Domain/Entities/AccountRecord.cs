using System;

namespace Desafio.Domain.Entities
{
    public class AccountRecord
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Operation { get; set; }
        public decimal Value { get; set; }
        public string Type { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalValue { get; set; }
    }
}
