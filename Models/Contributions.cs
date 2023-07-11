using FourtitudeIntegrated.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourtitudeIntegrated.Models
{
    public class Contributions
    {
        [Key]
        public string ContributionId { get; set; }
        public int AccountId { get; set; }
        public decimal AmountDue { get; set; }
        public decimal PenaltyDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get { return AmountDue + PenaltyDue - AmountPaid; } }
        public DateTime? DateOfLastPayment { get; set; }
        public DateTime? DateDue { get; set; }
        public ContributionStatus Status { get; set; }
        public Accounts Accounts { get; set; } = null!;
    }

    public class ContributionsDTO
    {
        public string ContributionId { get; set; }
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
        public string ContributionId { get; set; }
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
}
