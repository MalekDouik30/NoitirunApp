using Microsoft.EntityFrameworkCore;
using Noitirun.Core.DTOs.Country;
using Noitirun.Core.Exceptions;
using Noitirun.Core.Interfaces;

using System.Linq.Expressions;


namespace NoitirunApp.Application.Country.Queries
{

    public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, CountryDocumentairePaginator>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<GetCountryQueryHandler> _logger;

        public GetCountryQueryHandler(ICountryRepository countryRepository , ILogger<GetCountryQueryHandler> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }
        public async Task<CountryDocumentairePaginator> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation( "Get Countries");
            try
            {
                // Initialize criteria
                Expression<Func<Noitirun.Core.Entities.Country, bool>> criteria = null;

                // Build criteria based on request
                if (!string.IsNullOrEmpty(request.Name) ||
                    !string.IsNullOrEmpty(request.NameFr) ||
                    !string.IsNullOrEmpty(request.PhonePrefixe) ||
                    request.Id.HasValue ||
                    request.OnlyNotDeleted
                    )
                {
                    criteria = c =>
                        (string.IsNullOrEmpty(request.Name) || EF.Functions.Like(c.Name, $"%{request.Name}%")) &&
                        (string.IsNullOrEmpty(request.NameFr) || EF.Functions.Like(c.Name, $"%{request.NameFr}%")) &&
                        (string.IsNullOrEmpty(request.PhonePrefixe) || EF.Functions.Like(c.PhonePrefixe, $"%{request.PhonePrefixe}%")) &&
                        (!request.Id.HasValue || c.Id == request.Id) &&
                        (!request.OnlyNotDeleted || c.Status == true);
                }

                // Get total items based on criteria
                var totalItems = criteria == null
                    ? await _countryRepository.CountAsync()
                    : await _countryRepository.CountAsync(criteria);

                // Fetch paginated data
                var data = criteria == null
                    ? await _countryRepository.GetAllAsyncWithPagable(request.PageNumber, request.PageSize)
                    : await _countryRepository.FindAllAsync(
                        criteria,
                        request.PageNumber,
                        request.PageSize
                    );

                // Map to DTO
                var countries = data.Select(c => new CountryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    NameFr = c.NameFr,
                    PhonePrefixe = c.PhonePrefixe,
                    Status = c.Status,
                }).ToList();

                // Build and return paginator result
                var result = new CountryDocumentairePaginator
                {
                    Countries = countries,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalItems = totalItems
                };

                _logger.LogInformation("Successfully fetched countries with pagination.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get Countries : {ex.Message}");
                throw new BusinessException($"Erreur get Countries :  {ex.Message}");
            }
        }
    }
}
