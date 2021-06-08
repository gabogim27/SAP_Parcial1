namespace SAP.Crypto.Domain.Request
{
    using SAP.Crypto.Domain.Implementation;
    public class AccountRequest
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string DNI { get; set; }
        public string Alias { get; set; }
        public string BankName { get; set; }
        public string Location { get; set; }
    }
}
