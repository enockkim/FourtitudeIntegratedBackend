﻿using FourtitudeIntegrated.Enum;
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
        public LoanStatus Status { get; set; } = LoanStatus.Unpaid;
        public Accounts Accounts { get; set; }
    }

    public class NewLoanDTO
    {
        public int AccountId { get; set; }
        public decimal AmountBorrowed { get; set; }
        public decimal InterestRate { get; set; }
        public bool PenaltyStatus { get; set; } = false;
    }

    public class LoanPayment
    {
        public int LoanId { get; set; }
        public string UserId { get; set; }
        public decimal AmountPaid { get; set; }
        public string TransactionRef { get; set; }
        public string Description { get; set; }
        public DateTime DateOfPayment { get; set; }
    }
}
