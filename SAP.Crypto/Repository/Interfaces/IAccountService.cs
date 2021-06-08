namespace SAP.Crypto.Services.Interfaces
{
    using SAP.Crypto.Domain.Request;
    using SAP.Crypto.Domain.Response;

    public interface IAccountService
    {
        public int CreateAccount(AccountRequest request);
        AccountResponse GetAccount(AccountIdRequest idRequest);
        bool HasAccount(string dni);
        bool Deposit(TransferDepositRequest request);
        bool Transfer(TransferDepositRequest request);
        bool BuyCrypto(TransferDepositRequest request);
    }
}
