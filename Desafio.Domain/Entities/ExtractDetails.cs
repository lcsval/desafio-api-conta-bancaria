using System;

namespace Desafio.Domain.Entities
{
    public class ExtractDetails
    {
        public Guid Id { get; set; }
        public Guid ExtractId { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
