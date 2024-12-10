using AutoMapper;
using Azure.Core;
using Commun.Application.seedWork;
using Microsoft.EntityFrameworkCore;
using Noitirun.Core.DTOs.Country;
using Noitirun.Core.Entities;
using Noitirun.Core.Exceptions;
using Noitirun.Core.Interfaces;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NoitirunApp.Application.Country.Commands
{
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, CountryDTO>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCountryCommandHandler> _logger;
        private readonly ICountryRepository _countryRepository;
        private readonly IDateTimeService _dateTimeService;

        public UpdateCountryCommandHandler(
            ICountryRepository countryRepository, 
            ILogger<CreateCountryCommandHandler> logger,
            IMapper mapper, 
            IDateTimeService dateTimeService)
        {
            _countryRepository = countryRepository;
            _logger = logger;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public async Task<CountryDTO> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            if (request.Country.Id == null)
            {
                _logger.LogError("ID cannot be null");
                throw new ArgumentNullException(nameof(request.Country));
            }

            _logger.LogInformation("Update Country with Id: ", request.Country.Id);

            var oldCountry = await _countryRepository.GetByIdAsync(request.Country.Id ?? Guid.Empty);
            if (oldCountry == null)
            {
                _logger.LogError("Error in country ID not found in database");
                throw new NotFoundException("Error in country ID not found in database");
            }
            oldCountry = UpdateCountryFields(oldCountry, request.Country);
            _countryRepository.Update(oldCountry);

            _logger.LogInformation("Successfully updated country with ID: ", request.Country.Id);

            return _mapper.Map<CountryDTO>(oldCountry);
        }

        private Noitirun.Core.Entities.Country UpdateCountryFields(Noitirun.Core.Entities.Country oldCountry, CountryDTO newCountryData)
        {
            bool isUpdated = false;

            if (!string.IsNullOrEmpty(newCountryData.Name) && oldCountry.Name != newCountryData.Name)
            {
                oldCountry.Name = newCountryData.Name;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(newCountryData.PhonePrefixe) && oldCountry.PhonePrefixe != newCountryData.PhonePrefixe)
            {
                // Regex condition
                var regex = new Regex(@"^\+(\d{1,4})$");
                if (!regex.IsMatch(newCountryData.PhonePrefixe))
                {
                    _logger.LogWarning("Validation failed: Invalid phone prefix.");
                    throw new BusinessException("Invalid phone prefix.");
                }


                oldCountry.PhonePrefixe = newCountryData.PhonePrefixe;
                isUpdated = true;
            }

            if (newCountryData.Status.HasValue && oldCountry.Status != newCountryData.Status.Value)
            {
                oldCountry.Status = newCountryData.Status.Value;
                isUpdated = true;
            }

            if (isUpdated)
            {
                oldCountry.LastModified = _dateTimeService.Now;
            }
            return oldCountry;
        }
    }
}
