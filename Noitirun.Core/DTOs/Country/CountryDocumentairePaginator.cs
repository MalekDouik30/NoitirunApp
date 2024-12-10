using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noitirun.Core.DTOs.Country
{
    public class CountryDocumentairePaginator
    {
        public List<CountryDTO> Countries { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalItems { get; set; } = 0;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize); // Calcul automatique


    }
}
