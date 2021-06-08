namespace SAP.Crypto.Domain.Request
{
    using SAP.Crypto.Domain.Implementation;
    using System;
    public class AccountIdRequest
    {
        public int AccountNumber { get; set; }

        public Guid UUID { get; set; }

        public AccountType AccountType { get; set; }
    }
}
