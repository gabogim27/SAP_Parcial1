namespace SAP.Crypto.Domain.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CryptoAccount
    {
        [Key]
        public Guid UUID { get; set; }

        public decimal CryptoBalance { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public List<Transaction> Transaction { get; set; }

        public DateTime DateOpened { get; set; }

        public DateTime DateClosed { get; set; }
    }
}
