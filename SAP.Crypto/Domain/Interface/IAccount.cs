using SAP.Crypto.Domain.Implementation;
using System.Collections.Generic;

namespace Domain
{
    public interface IAccount
    {
        decimal Balance { get; set; }

        Customer Client { get; set; }

        List<Transaction> Transaction { get; set; }

        void Save();

        void Send();
    }
}
