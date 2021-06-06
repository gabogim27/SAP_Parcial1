namespace SAP.Crypto.Domain.Implementation
{
    using System;
    using System.Collections.Generic;

    public class CryptoAccount
    {
        public Guid UUID { get; set; }

        public decimal CurrentBalance { get; set; }

        public Customer Customer { get; set; }

        public AccountType AccountType { get; set; }

        public List<Transaction> Transaction { get; set; }

        public DateTime DateOpened { get; set; }

        public DateTime DateClosed { get; set; }
    }
}
