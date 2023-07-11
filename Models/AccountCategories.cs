using System.ComponentModel.DataAnnotations;

namespace FourtitudeIntegrated.Models
{
    public class AccountCategories
    {
        [Key]
        public int AccountCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; } = new List<Accounts>();
    }

    public class AccountCategoriesDTO
    {
        public int AccountCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
