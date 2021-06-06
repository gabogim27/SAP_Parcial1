namespace SAP.Crypto.Domain.Implementation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Invested { get; set; }

    }
}
