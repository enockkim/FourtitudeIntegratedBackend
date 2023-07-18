using FourtitudeIntegrated.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourtitudeIntegrated.Models
{
    public class ContributionPayments
    {
        [Key]
        public int ContributionPaymentsId { get; set; }
        [ForeignKey("Contributions")]
        public int ContributionId { get; set; }
        [ForeignKey("GeneralLedger")]
        public int GeneralLedgerEntry { get; set; }
        public string Comment { get; set; }
        public Contributions Contributions { get; set; }
        public GeneralLedger GeneralLedger { get; set; }
    }

    public class ContributionPaymentsDTO
    {
    }
  
}
