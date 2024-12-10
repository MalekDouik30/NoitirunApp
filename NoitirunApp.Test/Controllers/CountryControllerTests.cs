using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Noitirun.Core.DTOs.Country;
using NoitirunApp.Application.Country.Queries;
using NoitirunApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noitirun.Test.Controllers
{
    [TestClass]
    public class CountryControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CountryController _controller;
        public CountryControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new CountryController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task GetCountries_ShouldReturnOkResult_WithPaginatedCountries()
        {
            // Arrange
            var query = new GetCountryQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            var paginatedResult = new CountryDocumentairePaginator
            {
                Countries = new List<CountryDTO>
            {
                new CountryDTO { Id = Guid.NewGuid(), Name = "Country1" },
                new CountryDTO { Id = Guid.NewGuid(), Name = "Country2" }
            },
                PageNumber = 1,
                PageSize = 10,
                TotalItems = 2
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<GetCountryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedResult);

            // Act
            var result = await _controller.GetCountries(1, 10, null, null, null, null);

            // Assert
            Assert.IsNotNull(result, "The result should not be null.");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult), "The result is not of type OkObjectResult.");
            var okResult = (OkObjectResult)result; // Cast the result to OkObjectResult

            Assert.IsNotNull(okResult.Value, "The returned value should not be null.");
            Assert.IsInstanceOfType(okResult.Value, typeof(CountryDocumentairePaginator), "The returned value is not of type CountryDocumentairePaginator.");
            var returnedData = (CountryDocumentairePaginator)okResult.Value; // Cast to access properties if needed

            Assert.AreEqual(2, returnedData.Countries.Count);
            Assert.AreEqual("Country1", returnedData.Countries[0].Name);
        }
    }
}
