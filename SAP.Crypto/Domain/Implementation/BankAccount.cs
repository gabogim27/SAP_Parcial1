namespace SAP.Crypto.Domain.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BankAccount
    {
        [Key]
        public int AccountNumber { get; set; }

        public Bank BankInfo { get; set; }

        public int BankId { get; set; }

        public string CBU { get; set; }

        public string Alias { get; set; }

        public decimal CurrentArsBalance { get; set; }

        public decimal CurrentUsdBalance { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public DateTime DateOpened { get; set; }

        public DateTime DateClosed { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
