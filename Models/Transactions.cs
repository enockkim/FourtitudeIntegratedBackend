using FourtitudeIntegrated.Enum;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace FourtitudeIntegrated.Models
{
    public class Transactions
    {
        [Key]
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string TransactionRef { get; set; }
        public TransactionType TransactionType { get; set; }
        public Documents Document { get; set; }
        public ICollection<GeneralLedger> GeneralLedgerEntry { get; set; }
    }
    public class TransactionsDTO
    {
        public long TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string TransactionRef { get; set; }
        public TransactionType TransactionType { get; set; }
    }

    public class NewTransacton
    {
        public TransactionsDTO TransactionDetails { get; set; }
        public int? AccountFrom { get; set; }
        public int? AccountTo { get; set; }
    }

    public class ViewTransactionsDTO
    {
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionRef { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
    }
}
