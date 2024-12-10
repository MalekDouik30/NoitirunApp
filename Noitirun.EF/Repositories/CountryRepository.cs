using Microsoft.EntityFrameworkCore;
using Noitirun.Core.Entities;
using Noitirun.Core.Interfaces;
using NoitirunApp.Infrastructure.Data;


namespace Noitirun.EF.Repositories
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
