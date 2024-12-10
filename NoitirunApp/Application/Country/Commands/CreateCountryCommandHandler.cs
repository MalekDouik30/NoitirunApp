using AutoMapper;
using Commun.Application.seedWork;
using Microsoft.EntityFrameworkCore;
using Noitirun.Core.DTOs.Country;
using Noitirun.Core.Entities;
using Noitirun.Core.Exceptions;
using Noitirun.Core.Interfaces;
using System.Text.RegularExpressions;

namespace NoitirunApp.Application.Country.Commands
{
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, CountryDTO>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CreateCountryCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;

        public CreateCountryCommandHandler(ICountryRepository countryRepository, ILogger<CreateCountryCommandHandler> logger, IMapper mapper, IDateTimeService dateTimeService)
        {
            _countryRepository = countryRepository;
            _logger = logger;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public async Task<CountryDTO> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new country with Name: {Name}", request.Country.Name);
            try
            {
                if (string.IsNullOrWhiteSpace(request.Country.Name))
                {
                    _logger.LogWarning("Validation failed: Country Name is required.");
                    throw new BusinessException("Country Name is required.");
                }

                if (string.IsNullOrWhiteSpace(request.Country.PhonePrefixe))
                {
                    _logger.LogWarning("Validation failed: Country Phone Prefix is required.");
                    throw new BusinessException("Country Phone Prefix is required.");
                }

                // Validate the phone prefix using the regex
                var regex = new Regex(@"^\+(\d{1,4})$");
                if (!regex.IsMatch(request.Country.PhonePrefixe))
                {
                    _logger.LogWarning("Validation failed: Invalid phone prefix.");
                    throw new BusinessException("Invalid phone prefix.");
                }

                // If it passes validation, you can continue with the logic
                _logger.LogInformation("Phone prefix is valid --> Verification Done :) ");
            

                // Map the DTO to the entity
                var country = _mapper.Map<Noitirun.Core.Entities.Country>(request.Country);

                // Add additional metadata
                country.Status = true;
                country.Created = _dateTimeService.Now;
                country.LastModified = _dateTimeService.Now;

                // Add the new country to the repository
                country = await _countryRepository.AddAsync(country, cancellationToken);                
                _logger.LogInformation(message: "Country created successfully with Id: {Id}", country.Id);

                // Map the entity back to DTO
                return _mapper.Map<CountryDTO>(country);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error Creating Country: {ex.Message}");
                throw new DomainException($"Error Creating Country :  {ex.Message}");
            }
        }
    }
}
