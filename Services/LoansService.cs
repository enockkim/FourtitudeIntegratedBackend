using FourtitudeIntegrated.DbContexts;
using FourtitudeIntegrated.Models;
using Microsoft.EntityFrameworkCore;

namespace FourtitudeIntegrated.Services
{
    public class LoansService
    {
        private TransactionService transactionService;
        private readonly FourtitudeIntegratedContext context;

        public LoansService(TransactionService transactionService, FourtitudeIntegratedContext context)
        {
            this.transactionService = transactionService;
            this.context = context;
        }

        public async Task<Loans> PayLoan(LoanPayment loanPayment)
        {
            var loan = await context.Loans.FindAsync(loanPayment.LoanId);

            //Create new transaction
            var newTransaction = new NewTransacton()
            {
                AccountFrom = loan.AccountId,
                AccountTo = 9002, //Loans receivable account
                TransactionDetails = new TransactionsDTO()
                {
                    TransactionDate = loanPayment.DateOfPayment,
                    TransactionType = Enum.TransactionType.Transfer,
                    TransactionRef = loanPayment.TransactionRef,
                    UserId = loanPayment.UserId,
                    Amount = loanPayment.AmountPaid,
                    Description = "Loan payment: "+ loanPayment.Description
                }
            };

            var transaction = await transactionService.AddTransaction(newTransaction);

            //Create loan transaction
            GeneralLedger generalLedgerEntry = await context.GeneralLedger.Where(g => g.TransactionId == transaction.TransactionId && g.AccountId != loan.AccountId).FirstOrDefaultAsync();
            var loanTransaction = new LoanTransactions()
            {
                LoanId = loan.LoanId,
                GeneralLedgerEntry = generalLedgerEntry.EntryNo,
                LoanTransactionType = Enum.LoanTransactionType.Payment,
                Comment = loanPayment.Description
            };
            context.LoanTransactions.Add(loanTransaction);
            await context.SaveChangesAsync();

            //Update loan record
            loan.AmountPaid = loanPayment.AmountPaid + loan.AmountPaid;
            loan.DateOfLastPayment = loanPayment.DateOfPayment;
            loan.Status = loan.Balance == 0 ? Enum.LoanStatus.Cleared : Enum.LoanStatus.Partial;

            context.Entry(loan).State = EntityState.Modified;   
            
            await context.SaveChangesAsync();

            return loan;
        }
    }
}
