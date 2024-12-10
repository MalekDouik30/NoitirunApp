using Microsoft.Extensions.Logging;
using Moq;
using Noitirun.Core.Entities;
using Noitirun.Core.Interfaces;
using NoitirunApp.Application.Country.Queries;
using System.Linq.Expressions;

namespace Noitirun.Test
{
    [TestClass]
    public class GetCountryQueryHandlerTests
    {
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly Mock<ILogger<GetCountryQueryHandler>> _mockLogger;

        private readonly GetCountryQueryHandler _handler;

        public GetCountryQueryHandlerTests()
        {
            _mockCountryRepository = new Mock<ICountryRepository>();
            _mockLogger = new Mock<ILogger<GetCountryQueryHandler>>();
            _handler = new GetCountryQueryHandler(_mockCountryRepository.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnPaginatedCountries_WhenCriteriaIsMatched()
        {
            // Arrange 
            var query = new GetCountryQuery
            {
                PageNumber = 1,
                PageSize = 2,
                Name = "Test",
                OnlyNotDeleted = false
            };

            var countries = new List<Country>
            {
            new Country { Id = Guid.NewGuid(), Name = "Test1", Status = true },
            new Country { Id = Guid.NewGuid(), Name = "Test2", Status = true }
            };

            // Test Count
            _mockCountryRepository
                .Setup(repo => repo.CountAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                .ReturnsAsync(2);

            // Test elements in list
            _mockCountryRepository
                .Setup(repo => repo.FindAllAsync(It.IsAny<Expression<Func<Country, bool>>>(), query.PageNumber, query.PageSize))
                .ReturnsAsync(countries);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.TotalItems);
            Assert.AreEqual(2, result.Countries.Count);
            Assert.AreEqual("Test1", result.Countries[0].Name);
            Assert.AreEqual("Test2", result.Countries[1].Name);

            //_loggerMock.Verify(logger => logger.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);

        }



    }
}
