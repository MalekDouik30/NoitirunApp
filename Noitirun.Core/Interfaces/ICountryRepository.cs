using Noitirun.Core.Entities;


namespace Noitirun.Core.Interfaces
{
    public interface ICountryRepository :IBaseRepository<Country>
    {
        // Special Methode Here ! 
        //Task<IEnumerable<Country>> GetCountriesByRegionAsync(string region);
    }
}
