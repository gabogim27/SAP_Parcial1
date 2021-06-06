namespace SAP.Crypto.Domain.Implementation
{
    using System;
    public class BankAccount
    {
        public int AccountNumber { get; set; }

        public Bank BankInfo { get; set; }

        public int CBU { get; set; }

        public string Alias { get; set; }

        public decimal CurrentBalance { get; set; }

        public Customer Customer { get; set; }

        public AccountType AccountType { get; set; }

        public DateTime DateOpened { get; set; }

        public DateTime DateClosed { get; set; }
    }
}
