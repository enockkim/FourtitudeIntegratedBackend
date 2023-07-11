using FourtitudeIntegrated.Enum;
using System.ComponentModel.DataAnnotations;

namespace FourtitudeIntegrated.Models
{
    public class DocumentTypes
    {
        [Key]
        public int DocumentTypeId { get; set; }
        public DocumentTypes TypeName { get; set; }
        public string Description { get; set; }
        public ICollection<Documents> Documents { get; } = new List<Documents>();
    }
}
