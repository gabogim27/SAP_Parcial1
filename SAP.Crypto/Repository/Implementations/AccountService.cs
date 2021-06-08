namespace Services.Implementations
{
    using SAP.Crypto.Domain.Implementation;
    using SAP.Crypto.Domain.Request;
    using SAP.Crypto.Domain.Response;
    using SAP.Crypto.Repository.Interfaces;
    using SAP.Crypto.Services.Helpers;
    using SAP.Crypto.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class AccountService : IAccountService
    {
        private readonly IRepository<BankAccount> bankAccountRepository;

        private readonly IRepository<CryptoAccount> cryptoRepository;

        private readonly IRepository<Customer> customerRepository;

        private readonly IRepository<Bank> bankRepository;
        public AccountService(IRepository<BankAccount> bankAccountRepository,
                              IRepository<CryptoAccount> cryptoRepository,
                              IRepository<Customer> customerRepository,
                              IRepository<Bank> bankRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.cryptoRepository = cryptoRepository;
            this.customerRepository = customerRepository;
            this.bankRepository = bankRepository;
        }
        public int CreateAccount(AccountRequest request)
        {
            var bankAccNumber = GetAccountNumer();
            var cbu = IdGeneratorHelper.NextId().ToString();
            var customerId = IdGeneratorHelper.NextId();
            var customer = CreateCustomer(request, customerId);
            var bankId = IdGeneratorHelper.NextId();
            var bankAccount = new BankAccount()
            {
                AccountNumber = bankAccNumber,
                CurrentArsBalance = 0,
                CurrentUsdBalance = 0,
                CustomerId = customerId,
                BankId = bankId,
                Alias = !string.IsNullOrWhiteSpace(request.Alias) ? request.Alias : $"{request.Name}.{request.SurName}.{request.DNI}",
                CBU = !CheckAccountData(0, cbu, false) ? cbu : IdGeneratorHelper.NextId().ToString(),
                BankInfo = new Bank()
                {
                    BankId = bankId,
                    Name = request.BankName,
                    Location = request.Location
                },
                Customer = customer,
                DateOpened = DateTime.Now
            };

            var cryptoAccount = new CryptoAccount()
            {
                CryptoBalance = 0,
                CustomerId = customerId,
                Customer = customer,
                DateOpened = DateTime.Now,
                UUID = Guid.NewGuid()
            };

            bankAccountRepository.Save(bankAccount);
            cryptoRepository.Save(cryptoAccount);

            return bankAccNumber;
        }

        private Customer CreateCustomer(AccountRequest request, int customerId)
        {
            return new Customer()
            {
                CustomerId = customerId,
                DNI = request.DNI,
                Email = request.Email,
                Name = request.Name,
                SurName = request.SurName,
                Telephone = request.Telephone
            };
        }

        public AccountResponse GetAccount(AccountIdRequest idRequest)
        {
            var response = new AccountResponse();

            if (idRequest.AccountType == AccountType.ARS || idRequest.AccountType == AccountType.Dollars)
            {
                var accountToReturn = bankAccountRepository.FirstOrDefault(x => x.AccountNumber == idRequest.AccountNumber);
                var accounCustomer = customerRepository.FirstOrDefault(x => x.CustomerId == accountToReturn.CustomerId);
                var bankInfo = bankRepository.FirstOrDefault(x => x.BankId == accountToReturn.BankId);
                if (accountToReturn != null && accounCustomer != null)
                {
                    response.AccountNumber = accountToReturn.AccountNumber;
                    response.Alias = accountToReturn.Alias;
                    response.CBU = accountToReturn.CBU;
                    response.ArsBalance = accountToReturn.CurrentArsBalance;
                    response.UsdBalance = accountToReturn.CurrentUsdBalance;
                    response.BankInfo = bankInfo;
                    response.Customer = accounCustomer;
                    response.Transactions = accountToReturn.Transactions;
                }
            }
            if (idRequest.AccountType == AccountType.Crypto)
            {
                var cryptoToReturn = cryptoRepository.FirstOrDefault(x => x.UUID == idRequest.UUID);
                var customer = customerRepository.FirstOrDefault(x => x.CustomerId == cryptoToReturn.CustomerId);
                
                if (cryptoToReturn != null && customer != null)
                {
                    response.UUID = cryptoToReturn.UUID;
                    response.CryptoBalance = cryptoToReturn.CryptoBalance;
                    response.Customer = customer;
                    response.Transactions = cryptoToReturn.Transaction;
                }
            }

            return response;
        }

        public bool HasAccount(string dni)
        {
            return bankAccountRepository.Where(x => x.Customer.DNI == dni)?.Count() > 0;
        }

        public bool Deposit(TransferDepositRequest request)
        {
            if (request.AccountType == AccountType.ARS || request.AccountType == AccountType.Dollars)
            {
                return DepositMoney(request);
            }
            else
            {
                return DepositCrypto(request);
            }
        }

        public bool Transfer(TransferDepositRequest request)
        {
            if (request.AccountType == AccountType.ARS || request.AccountType == AccountType.Dollars)
            {
                var id = !string.IsNullOrWhiteSpace(request.CBU) ? request.CBU : request.Alias;
                var bankCustomer = customerRepository.FirstOrDefault(x => x.DNI == request.SourceDni);
                var fromAccount = bankAccountRepository.FirstOrDefault(x => x.CustomerId == bankCustomer.CustomerId);
                var toAccount = bankAccountRepository.Where(x => x.CBU == id || x.Alias == id)?.FirstOrDefault();

                if (fromAccount != null && toAccount != null)
                {
                    DoDebitCreditOperation(request, fromAccount, true);
                    DoDebitCreditOperation(request, toAccount);
                    var updateFrom = UpdateBankAccount(fromAccount);
                    var updateTo = UpdateBankAccount(toAccount);
                    return updateFrom && updateTo;
                }
            }
            else
            {
                var fromAccount = cryptoRepository.Where(x => x.Customer.DNI == request.SourceDni)?.FirstOrDefault();
                var toAccount = cryptoRepository.Where(x => x.UUID == request.UUID)?.FirstOrDefault();

                if (fromAccount != null && toAccount != null)
                {
                    DoCryptoDebitCreditOperation(request, fromAccount, true);
                    DoCryptoDebitCreditOperation(request, toAccount);
                    var updateFrom = UpdateCryptoAccount(fromAccount);
                    var updateTo = UpdateCryptoAccount(toAccount);
                    return updateFrom && updateTo;
                }
            }

            return false;
        }

        public bool BuyCrypto(TransferDepositRequest request)
        {
            var fromAccount = bankAccountRepository.Where(x => x.Customer.DNI == request.SourceDni)?.FirstOrDefault();
            var fromCryptoAccount = cryptoRepository.Where(x => x.UUID == request.UUID)?.FirstOrDefault();

            if (fromAccount != null && fromCryptoAccount != null)
            {
                var dollarConverted = CurrencyConverterHelper.ConvertMoney(CurrencyType.ARS, CurrencyType.USD, request.Amount);
                var btcConverted = CurrencyConverterHelper.ConvertMoney(CurrencyType.USD, CurrencyType.BTC, dollarConverted);
                DoDebitCreditOperation(request, fromAccount, true);
                request.Amount = btcConverted;
                DoCryptoDebitCreditOperation(request, fromCryptoAccount);
                var updateBank = UpdateBankAccount(fromAccount);
                var updateCrypto = UpdateCryptoAccount(fromCryptoAccount);
                return updateBank && updateCrypto;
            }

            return false;
        }

        private bool UpdateBankAccount(BankAccount fromAccount)
        {
            return bankAccountRepository.Update(fromAccount);
        }

        private bool UpdateCryptoAccount(CryptoAccount fromAccount)
        {
            return cryptoRepository.Update(fromAccount);
        }

        private void DoDebitCreditOperation(TransferDepositRequest request, BankAccount fromAccount, bool debit = false)
        {
            switch (request.AccountType)
            {
                case AccountType.ARS:
                    if (debit)
                    {
                        fromAccount.CurrentArsBalance -= request.Amount;
                    }
                    else
                    {
                        fromAccount.CurrentArsBalance += request.Amount;
                    }
                    break;
                case AccountType.Dollars:
                    if (debit)
                    {
                        fromAccount.CurrentUsdBalance -= request.Amount;
                    }
                    else
                    {
                        fromAccount.CurrentUsdBalance += request.Amount;
                    }
                    break;
            }
        }

        private void DoCryptoDebitCreditOperation(TransferDepositRequest request, CryptoAccount fromAccount, bool debit = false)
        {

            if (debit)
            {
                fromAccount.CryptoBalance -= request.Amount;
            }
            else
            {
                fromAccount.CryptoBalance += request.Amount;
            }
        }

        private bool DepositMoney(TransferDepositRequest request)
        {
            var idToFetch = !string.IsNullOrWhiteSpace(request.CBU) ? request.CBU : request.Alias;
            var accountToDeposit = bankAccountRepository.FirstOrDefault(x => x.CBU == request.CBU || x.Alias == request.Alias);

            if (accountToDeposit != null)
            {
                DoDebitCreditOperation(request, accountToDeposit);
                return UpdateBankAccount(accountToDeposit);
            }

            return false;
        }

        public bool DepositCrypto(TransferDepositRequest request)
        {
            if (request.AccountType == AccountType.Crypto)
            {
                var accountToDeposit = cryptoRepository.FirstOrDefault(x => x.UUID == request.UUID);

                if (accountToDeposit != null)
                {
                    DoCryptoDebitCreditOperation(request, accountToDeposit);
                    return UpdateCryptoAccount(accountToDeposit);
                }
            }

            return false;
        }

        private int GetAccountNumer()
        {
            var number = ReturnNumber();

            if (!CheckAccountData(number, string.Empty, true))
            {
                return number;
            }

            return ReturnNumber();
        }

        private int ReturnNumber()
        {
            var startWith = "32";
            var generator = new Random();
            var r = generator.Next(0, 999999).ToString("D6");
            var number = Convert.ToInt32(startWith + r);
            return number;
        }

        private bool CheckAccountData(int accountNumber, string cbu, bool checkAccount = false)
        {
            if (checkAccount)
            {
                var list = bankAccountRepository.List().Select(x => x.AccountNumber).ToList();
                return list.Contains(accountNumber);
            }
            else
            {
                var list = bankAccountRepository.List().Select(x => x.CBU).ToList();
                return list.Contains(cbu);
            }
        }
    }
}
