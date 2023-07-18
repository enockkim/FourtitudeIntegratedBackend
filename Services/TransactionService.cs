using FourtitudeIntegrated.DbContexts;
using FourtitudeIntegrated.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourtitudeIntegrated.Services
{
    public class TransactionService
    {
        private readonly FourtitudeIntegratedContext _context;

        public TransactionService(FourtitudeIntegratedContext context)
        {
            _context = context;
        }

        public async Task<ViewTransactionsDTO> AddTransaction(NewTransacton NewTransaction)
        {
            List<GeneralLedger> GeneralLedgerEntries = new List<GeneralLedger>();

            //Create a transaction record
            Transactions Transactions = _context.Transactions.Add(new Transactions()
            {
                TransactionDate = NewTransaction.TransactionDetails.TransactionDate,
                CreatedDate = DateTime.Now,
                TransactionType = NewTransaction.TransactionDetails.TransactionType,
                Description = NewTransaction.TransactionDetails.Description,
                TransactionRef = NewTransaction.TransactionDetails.TransactionRef,
            }).Entity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //Create a general ledger entry
            if (NewTransaction.TransactionDetails.TransactionType == Enum.TransactionType.Transfer)
            {
                //Debit
                var debit = new GeneralLedger()
                {
                    EntryNo = 0,
                    AccountId = (int)NewTransaction.AccountTo,
                    Amount = NewTransaction.TransactionDetails.Amount,
                    UserId = NewTransaction.TransactionDetails.UserId,
                    TransactionId = Transactions.TransactionId,
                    EntryType = Enum.EntryType.Debit,
                    EntryDateTime = DateTime.Now
                };
                GeneralLedgerEntries.Add(debit);

                //Credit
                var credit = new GeneralLedger()
                {
                    EntryNo = 0,
                    AccountId = (int)NewTransaction.AccountFrom,
                    Amount = NewTransaction.TransactionDetails.Amount,
                    UserId = NewTransaction.TransactionDetails.UserId,
                    TransactionId = Transactions.TransactionId,
                    EntryType = Enum.EntryType.Credit,
                    EntryDateTime = DateTime.Now
                };
                GeneralLedgerEntries.Add(credit);
            }
            else
            {
                var DepositOrWithdrawal = new GeneralLedger()
                {
                    AccountId = (int)(NewTransaction.AccountFrom ?? NewTransaction.AccountTo),
                    Amount = NewTransaction.TransactionDetails.Amount,
                    UserId = NewTransaction.TransactionDetails.UserId,
                    TransactionId = Transactions.TransactionId,
                    EntryType = NewTransaction.TransactionDetails.TransactionType == Enum.TransactionType.Withdrawal ? Enum.EntryType.Credit : Enum.EntryType.Debit,
                    EntryDateTime = DateTime.Now
                };
                GeneralLedgerEntries.Add(DepositOrWithdrawal);
            }

            await _context.GeneralLedger.AddRangeAsync(GeneralLedgerEntries);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            ViewTransactionsDTO viewTransactionsDTO = new ViewTransactionsDTO()
            {
                TransactionId = Transactions.TransactionId,
                TransactionDate = Transactions.TransactionDate,
                TransactionRef = Transactions.TransactionRef,
                Description = Transactions.Description,
                TransactionType = Transactions.TransactionType.ToString(),
                Amount = NewTransaction.TransactionDetails.Amount,
                //AccountId = Transactions.
            };

            return viewTransactionsDTO;
        }
    }
}
