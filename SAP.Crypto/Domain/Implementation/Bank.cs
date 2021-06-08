using System.ComponentModel.DataAnnotations;

namespace SAP.Crypto.Domain.Implementation
{
    public class Bank
    {
        [Key]
        public int BankId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
