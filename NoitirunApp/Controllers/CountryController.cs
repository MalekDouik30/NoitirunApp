using Microsoft.AspNetCore.Mvc;
using NoitirunApp.Application.Country.Queries;
using MediatR;
using NoitirunApp.Application.Country.Commands;
using Noitirun.Core.DTOs.Country;

namespace NoitirunApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries(int pageNumber, int pageSize, string? name, string? nameFr, string? id, string? phonePrefix)
        {
            Guid? idGuid = null;
            if (!string.IsNullOrEmpty(id))
            {
                idGuid = new Guid(id);  // Convertir l'ID en Guid
            }
            var result = await _mediator.Send(new GetCountryQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Name = name,
                NameFr = nameFr,
                PhonePrefixe = phonePrefix,
                Id = idGuid,
                OnlyNotDeleted = false
            });
            return Ok(result);
        }

        [HttpGet("GetActiveCountries")]
        public async Task<IActionResult> GetActiveCountries(int pageNumber, int pageSize, string? name, string? id, string? phonePrefix)
        {
            Guid? idGuid = null;
            if (!string.IsNullOrEmpty(id))
            {
                idGuid = new Guid(id);  // Convertir l'ID en Guid
            }
            var result = await _mediator.Send(new GetCountryQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Name = name,
                PhonePrefixe = phonePrefix,
                Id = idGuid,
                OnlyNotDeleted = true // Fetch only not-deleted countries
            });
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddCountry([FromForm] CountryDTO dto , CancellationToken cancellationToken)
        {
            var createdCountry = await _mediator.Send(new CreateCountryCommand(dto), cancellationToken);
            return Created("created successfully", createdCountry);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCountry([FromForm] CountryDTO dto)
        {
            var result = await _mediator.Send(new UpdateCountryCommand(dto));
            return Ok(result);
        }

        [HttpPut("ChangeStatusCountry")]
        public async Task<IActionResult> ChangeStatusCountry(Guid id )
        {
            var result = await _mediator.Send(new ChangeStatusCountryCommand()
            {
                idCountry = id,
            });
            return Ok(result);
        }
    }
}
