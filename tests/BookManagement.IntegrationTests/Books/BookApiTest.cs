using BookManagement.API.Modules.Books.Api.Dtos;
using BookManagement.API.Modules.History.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using  FluentAssertions;
using Xunit;

namespace BookManagement.API.Tests.Integration;

[Collection("Integration")]
public class BookApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public BookApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateBook_ValidData_ReturnsCreated()
    {
        // Arrange
        var request = new BookRequestModel
        {
            Title = "Clean Architecture",
            ShortDescription = "A Craftsman's Guide to Software Structure and Design",
            PublishDate = new DateTime(2008, 8, 11),
            Authors = new List<string> { "Robert C. Martin" }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/books", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdBook = await response.Content.ReadFromJsonAsync<BookResponseModel>();
        createdBook.Should().NotBeNull();
        createdBook!.Title.Should().Be("Clean Architecture");
        createdBook.Authors.Should().Contain("Robert C. Martin");
    }

    [Fact]
    public async Task CreateBook_EmptyAuthors_ReturnsBadRequest()
    {
        // Arrange
        var request = new BookRequestModel
        {
            Title = "Invalid Book",
            PublishDate = DateTime.UtcNow,
            Authors = new List<string> { "" }  // Triggers HasNonEmptyStringsAttribute
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/books", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var errorContent = await response.Content.ReadAsStringAsync();
        errorContent.Should().Contain("non-empty string");
    }

    [Fact]
    public async Task CreateBook_TriggersHistoryEvent()
    {
        // Arrange
        var request = new BookRequestModel
        {
            Title = "Event Test Book",
            ShortDescription = "Triggers history entry",
            PublishDate = DateTime.UtcNow,
            Authors = new List<string> { "Test Author" }
        };

        // Act - Create book
        var response = await _client.PostAsJsonAsync("/api/v1/books", request);
        response.EnsureSuccessStatusCode();

        // Assert - History table populated by event handler
        using var scope = _factory.Services.CreateScope();
        var historyContext = scope.ServiceProvider.GetRequiredService<BookHistoryDbContext>();

        var historyEntries = await historyContext.BookHistories
            .Where(h => h.BookTitle == "Event Test Book")
            .ToListAsync();

        historyEntries.Should().HaveCount(1);
        historyEntries[0].Action.Should().Be("Created");
        historyEntries[0].BookTitle.Should().Be("Event Test Book");
        historyEntries[0].Authors.Should().Be("Test Author");
    }

    [Fact]
    public async Task CreateBook_MultipleAuthors_SavesCorrectly()
    {
        // Arrange
        var request = new BookRequestModel
        {
            Title = "The Pragmatic Programmer",
            ShortDescription = "From Journeyman to Master",
            PublishDate = new DateTime(1999, 10, 1),
            Authors = new List<string> { "Andrew Hunt", "David Thomas" }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/books", request);
        response.EnsureSuccessStatusCode();

        // Assert - Both book and history created
        using var scope = _factory.Services.CreateScope();
        var historyContext = scope.ServiceProvider.GetRequiredService<BookHistoryDbContext>();

        var historyEntry = await historyContext.BookHistories
            .FirstOrDefaultAsync(h => h.BookTitle == "The Pragmatic Programmer");

        historyEntry.Should().NotBeNull();
        historyEntry!.Authors.Should().Be("Andrew Hunt, David Thomas");
    }
}
