using Noitirun.Core.DTOs.Country;

namespace NoitirunApp.Application.Country.Commands
{
    public class UpdateCountryCommand :IRequest<CountryDTO>
    {
        public CountryDTO Country { get; set; }

        public UpdateCountryCommand(CountryDTO dto)
        {
            Country = dto;
        }
    }
}
