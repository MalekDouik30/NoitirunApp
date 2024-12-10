using Noitirun.Core.DTOs.Country;

namespace NoitirunApp.Application.Country.Queries
{
    public class GetCountryQuery : IRequest<CountryDocumentairePaginator>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? NameFr { get; set; }
        public string? PhonePrefixe { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool OnlyNotDeleted { get; set; } // Flag for not-deleted countries
    }
}
