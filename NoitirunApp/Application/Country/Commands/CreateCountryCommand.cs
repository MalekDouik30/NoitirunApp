using Noitirun.Core.DTOs.Country;

namespace NoitirunApp.Application.Country.Commands
{
    public class CreateCountryCommand:IRequest<CountryDTO>
    {
        public CountryDTO Country { get; set; }

        public CreateCountryCommand(CountryDTO country)
        {
            Country = country;
        }
    }
}
