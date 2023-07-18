using FourtitudeIntegrated.Models;

namespace FourtitudeIntegrated.Cache
{
    public class LocalCache //TODO implement redis
    {
        public static List<GetAccountsDto> AccountsCache = new List<GetAccountsDto>();
        public static List<AccountTypesDTO> AccountTypesCache = new List<AccountTypesDTO>();
        public static List<AccountCategoriesDTO> AccountCategoriesCache = new List<AccountCategoriesDTO>();
        public static List<ContributionsDTO> ContributionsCache = new List<ContributionsDTO>();
        public static List<Loans> LoansCache = new List<Loans>();
        public static List<LoanTransactions> LoanTransactionsCache = new List<LoanTransactions>();
    }
}
