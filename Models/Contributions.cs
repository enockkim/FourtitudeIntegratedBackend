using FourtitudeIntegrated.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourtitudeIntegrated.Models
{
    public class Contributions
    {
        [Key]
        public int ContributionId { get; set; }
        [ForeignKey("Accounts")]
        public int AccountId { get; set; }
        public decimal AmountDue { get; set; }
        public decimal PenaltyDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get { return AmountDue + PenaltyDue - AmountPaid; } }
        public bool PenaltyWaived { get; set; } = false;
        public DateTime? DateOfLastPayment { get; set; }
        public DateTime? DateDue { get; set; }
        public ContributionStatus Status { get; set; }
        public Accounts Accounts { get; set; }
    }

    public class ContributionsDTO
    {
        public int ContributionId { get; set; }
        public int AccountId { get; set; }
        public decimal AmountDue { get; set; }
        public decimal PenaltyDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get { return AmountDue + PenaltyDue - AmountPaid; } }
        public DateTime? DateOfLastPayment { get; set; }
        public DateTime? DateDue { get; set; }
        public ContributionStatus Status { get; set; }
    }
    public class ContributionsDTODetails
    {
        public int ContributionId { get; set; }
        public int AccId { get; set; }
        public string AccName { get; set; }
        public decimal AmountDue { get; set; }
        public decimal PenaltyDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get { return AmountDue + PenaltyDue - AmountPaid; } }
        public DateTime? DateOfLastPayment { get; set; }
        public DateTime? DateDue { get; set; }
        public string Status { get; set; }
    }
    public class ContributionsPayment
    {
        public int ContributionId { get; set; }
        public int AccountId { get; set; }
        public string UserId { get; set; }
        public decimal AmountPaid { get; set; }
        public string TransactionRef { get; set; }
        public DateTime DateOfPayment { get; set; }
    }
}
