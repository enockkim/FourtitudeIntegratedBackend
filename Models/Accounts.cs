using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourtitudeIntegrated.Models
{
    public class Accounts
    {
        [Key]
        public int AccountId { get; set; }
        [ForeignKey("AccountTypes")]
        public int AccountTypeId { get; set; }
        [ForeignKey("AccountCategories")]
        public int AccountCategoryId { get; set; }
        public string AccountName { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public int AccountStatus { get; set; }
        public AccountTypes AccountTypes { get; set; } = null!;
        public AccountCategories AccountCategory { get; set; } = null!;
        public ICollection<GeneralLedger> GeneralLedgerEntries { get; set; }
        public ICollection<Contributions> Contributions { get; set; }
    }

    public class AccountsDTO
    {
        public int AccountId { get; set; }
        public int AccountTypeId { get; set; }
        public int AccountCategoryId { get; set; }
        public string AccountName { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public int AccountStatus { get; set; }
    }
    public class GetAccountsDto
    {
        public int AccId { get; set; }
        public string AccType { get; set; }
        public string AccCategory { get; set; }
        public string AccName { get; set; }
        public decimal AccBalance { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public string AccStatus { get; set; }
    }
}
