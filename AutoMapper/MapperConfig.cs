using AutoMapper;
using FourtitudeIntegrated.Models;

namespace FourtitudeIntegrated.AutoMapper
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountTypes, AccountTypesDTO>();
                cfg.CreateMap<AccountCategories, AccountCategoriesDTO>();
                cfg.CreateMap<ContributionsDTO, Contributions>();
                //cfg.CreateMap<List<Contributions>, List<ContributionsDTO>>();
            });
            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
