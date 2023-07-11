using FourtitudeIntegrated.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace FourtitudeIntegrated.Models
{
    public class GeneralLedger
    {
        [Key]
        public int EntryNo { get; set; }
        [ForeignKey("Transactions")]
        public long TransactionId { get; set; }
        [ForeignKey("Accounts")]
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public EntryType EntryType { get; set; }
        public virtual Transactions Transactions { get; set; }
        public Accounts Accounts { get; set; }
    }
}
