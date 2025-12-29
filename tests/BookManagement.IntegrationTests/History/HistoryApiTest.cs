using FluentAssertions;
using Xunit;

namespace BookManagement.IntegrationTests.History
{
    [Collection("Integration")]
    public class HistoryApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
      
        public HistoryApiTests(CustomWebApplicationFactory factory)
        {
           
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetHistory_NoFilter_ReturnsAllResults()
        {
            var response = await _client.GetAsync("/api/v1/history?pageNumber=1&pageSize=10");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("items");
            content.Should().Contain("totalCount");
        }
    }
}
