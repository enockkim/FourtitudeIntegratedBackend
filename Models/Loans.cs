using FourtitudeIntegrated.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourtitudeIntegrated.Models
{
    public class Loans
    {
        [Key]
        public int LoanId { get; set; }
        [ForeignKey("Accounts")]
        public int AccountId { get; set; }
        public decimal AmountBorrowed { get; set; }
        public decimal InterestRate { get; set; }
        public decimal InterestDue { get; set; }
        public decimal PenaltyDue { get; set; }
        public decimal AmountPaid { get; set; }
        public bool PenaltyStatus { get; set; } = false;
        public decimal Balance { get { return AmountBorrowed + PenaltyDue + InterestDue - AmountPaid; } }
        public DateTime? DateOfLastPayment { get; set; }
        public DateTime? DateDue { get; set; }
        public LoanStatus Status { get; set; }
        public Accounts Accounts { get; set; }
    }

    public class LoansDTO
    {
    }
}
