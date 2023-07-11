using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace FourtitudeIntegrated.Models
{
    public class Documents
    {
        [Key]
        public int DocumentId { get; set; }
        [ForeignKey("Transactions")]
        public long TransactionId { get; set; }
        [ForeignKey("Documents")]
        public int DocumentTypeId { get; set; }
        public DateOnly DocumentDate { get; set; }
        public int DocumentNo { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UptdatedBy { get; set; }
        public virtual Transactions Transactions { get; set; }
        public DocumentTypes DocumentTypes { get; set; }
    }
}
