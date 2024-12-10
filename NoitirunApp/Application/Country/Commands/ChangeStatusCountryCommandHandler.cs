
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Noitirun.Core.Exceptions;
using Noitirun.Core.Interfaces;

namespace NoitirunApp.Application.Country.Commands
{
    public class ChangeStatusCountryCommandHandler : IRequestHandler<ChangeStatusCountryCommand, bool>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<ChangeStatusCountryCommandHandler> _logger;

        public ChangeStatusCountryCommandHandler(ICountryRepository countryRepository, ILogger<ChangeStatusCountryCommandHandler> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }
        public async Task<bool> Handle(ChangeStatusCountryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Delete the country with the status update that has the ID: {request.idCountry}");
            try
            {
                var country = await _countryRepository.GetByIdAsync(request.idCountry);
                if (country == null)
                {
                    _logger.LogError("Error in country ID not found in database");
                    throw new NotFoundException("Error in country ID not found in database");
                }

                country.Status = !country.Status; 
                _countryRepository.Update(country);
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error Delete Country: {ex.Message}");
                throw new DomainException($"Error Delete Country :  {ex.Message}");
            }
        }
    }
}
