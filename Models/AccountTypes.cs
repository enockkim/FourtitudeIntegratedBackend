using System.ComponentModel.DataAnnotations;

namespace FourtitudeIntegrated.Models
{
    public class AccountTypes
    {
        [Key]
        public int AccountTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; } = new List<Accounts>();
    }

    public class AccountTypesDTO
    {
        public int AccountTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }
}
