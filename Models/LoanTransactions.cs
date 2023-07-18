using FourtitudeIntegrated.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourtitudeIntegrated.Models
{
    public class LoanTransactions
    {
        [Key]
        public int LoanTransactionId { get; set; }
        [ForeignKey("Loans")]
        public int LoanId { get; set; }
        [ForeignKey("GeneralLedger")]
        public int GeneralLedgerEntry { get; set; }
        public LoanTransactionType LoanTransactionType { get; set; }
        public string Comment { get; set; }
        public Loans Loans { get; set; }
        public GeneralLedger GeneralLedger { get; set; }
    }

    public class LoanTransactionsDTO
    {
    }
}
