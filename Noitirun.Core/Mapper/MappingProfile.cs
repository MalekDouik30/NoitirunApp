using AutoMapper;
using Noitirun.Core.DTOs.Country;
using Noitirun.Core.Entities;


namespace Noitirun.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CountryDTO, Country>();
            CreateMap<Country, CountryDTO>();

        }
    }
}
