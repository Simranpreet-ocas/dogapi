using DogApi.Endpoints.ListAllBreeds;
using Flagsmith;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DogApi.UnitTests.Endpoints
{
    public class ListAllBreedsEndpointTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<ListAllBreedsEndpoint>> _mockLogger;
        private readonly Mock<IFlagsmithClient> _mockFlagsmithClient;
        private readonly ListAllBreedsEndpoint _endpoint;
        private readonly DefaultHttpContext _httpContext;

        public ListAllBreedsEndpointTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Use HttpClient with mocked HttpMessageHandler
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://dog.ceo/api/")
            };
            _mockLogger = new Mock<ILogger<ListAllBreedsEndpoint>>();

            _mockFlagsmithClient = new Mock<IFlagsmithClient>();

            _endpoint = new ListAllBreedsEndpoint(_httpClient, _mockLogger.Object, _mockFlagsmithClient.Object);
            _httpContext = new DefaultHttpContext();
            _endpoint.GetType().GetProperty("HttpContext").SetValue(_endpoint, _httpContext);
        }

        [Fact]
        public async Task HandleAsync_FeatureFlagDisabled_ReturnsForbidden()
        {
            // Arrange
            var mockFlags = new Mock<IFlags>();
            mockFlags.Setup(flags => flags.IsFeatureEnabled(It.IsAny<string>())).ReturnsAsync(false);
            _mockFlagsmithClient.Setup(client => client.GetEnvironmentFlags()).ReturnsAsync(mockFlags.Object);

            var request = new ListAllBreedsRequest();

            // Act
            await _endpoint.HandleAsync(request, CancellationToken.None);

            // Assert
            Assert.Equal(StatusCodes.Status403Forbidden, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task HandleAsync_SuccessfulFetch_ReturnsOk()
        {
            // Arrange: Mock the feature flag to return true
            var mockFlags = new Mock<IFlags>();
            mockFlags.Setup(flags => flags.IsFeatureEnabled(It.IsAny<string>())).ReturnsAsync(true);
            _mockFlagsmithClient.Setup(client => client.GetEnvironmentFlags()).ReturnsAsync(mockFlags.Object);

            // Mock Dog API response
            var breedsResponse = new DogApiResponse
            {
                Message = new Dictionary<string, List<string>>
                {
                    { "retriever", new List<string>() },
                    { "bulldog", new List<string>() }
                }
            };
            var jsonResponse = JsonSerializer.Serialize(breedsResponse);

            // Mock HttpClient using HttpMessageHandler
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            var request = new ListAllBreedsRequest { Page = 1, PageSize = 10, Search = "retriever" };

            // Act: Call the endpoint
            await _endpoint.HandleAsync(request, CancellationToken.None);

            // Assert: Response should be 200 OK
            Assert.Equal(StatusCodes.Status200OK, _httpContext.Response.StatusCode);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ListAllBreedsResponse>(responseBody);

            Assert.NotNull(response);
            Assert.Single(response.Breeds);
            Assert.Equal("retriever", response.Breeds[0]);
            Assert.Equal(2, response.TotalCount);
        }

        //[Fact]
        //public async Task HandleAsync_ApiFailure_ThrowsException()
        //{
        //    // Arrange
        //    var mockFlags = new Mock<IFlags>();
        //    mockFlags.Setup(flags => flags.IsFeatureEnabled(It.IsAny<string>())).ReturnsAsync(true);
        //    _mockFlagsmithClient.Setup(client => client.GetEnvironmentFlags()).ReturnsAsync(mockFlags.Object);

        //    _mockHttpClient.Setup(client => client.GetStringAsync("https://dog.ceo/api/breeds/list/all"))
        //        .ThrowsAsync(new HttpRequestException("API call failed"));

        //    var request = new ListAllBreedsRequest();

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => _endpoint.HandleAsync(request, CancellationToken.None));
        //    _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Failed to fetch breeds from the Dog API: {Message}", "API call failed"), Times.Once);
        //}
    }
}

