using BookManagement.API.Modules.Books.Api.Dtos;
using BookManagement.API.Modules.Books.Application.Interfaces;
using BookManagement.API.Modules.Books.Application.Models;
using BookManagement.API.Modules.Books.Domain.Entities;
using BookManagement.API.Modules.Books.Domain.Events;
using BookManagement.API.Shared.Events;
using BookManagement.API.Shared.Models;
using System.Text.Json;

namespace BookManagement.API.Modules.Books.Application.Services;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly IFailedEventStore _failedEventStore;
    private readonly IEventBus _eventBus;
    private readonly ILogger<BookService> _logger;

    public BookService(
        IBookRepository repository,
        IFailedEventStore failedEventStore,
        IEventBus eventBus,
        ILogger<BookService> logger)
    {
        _repository = repository;
        _failedEventStore = failedEventStore;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task<PagedResult<BookResponseModel>> GetPagedAsync(BookQuery query, CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching books page {PageNumber}", query.PageNumber);

        var result = await _repository.GetPagedAsync(query, ct);

        var responseItems = result.Items.Select(MapToResponseModel).ToList();

        return new PagedResult<BookResponseModel>(
            responseItems,
            result.TotalCount,
            result.Page,
            result.PageSize);
    }

    public async Task<BookResponseModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching book {BookId}", id);

        var book = await _repository.GetByIdAsync(id, ct);
        return book is null ? null : MapToResponseModel(book);
    }

    public async Task<BookResponseModel> CreateAsync(BookRequestModel request, CancellationToken ct = default)
    {
        _logger.LogInformation("Creating book {Title}", request.Title);

        var book = new BookManagement.API.Modules.Books.Domain.Entities.Book(
            title: request.Title,
            shortDescription: request.ShortDescription,
            publishDate: request.PublishDate,
            authors: request.Authors);

        await _repository.AddAsync(book);
        await _repository.SaveChangesAsync(ct);


        var bookSnapshot = new BookSnapshot(
        book.Id,
        book.Title,
        book.Authors
    );

        await PublishWithFallbackAsync(new BookCreatedEvent(bookSnapshot), ct);

        _logger.LogInformation("Created book {BookId}", book.Id);
        return MapToResponseModel(book);
    }

    public async Task<BookResponseModel?> UpdateAsync(Guid id, BookRequestModel request, CancellationToken ct = default)
    {
        _logger.LogInformation("Updating book {BookId}", id);

        var exisitngBook = await _repository.GetByIdAsync(id, ct);
        if (exisitngBook is null)
        {
            _logger.LogWarning("Book {BookId} not found", id);
            return null;
        }

        var oldSnapshot = new BookSnapshot(
                           exisitngBook.Id,
                           exisitngBook.Title,
                           exisitngBook.Authors
                         );

        exisitngBook.Title = request.Title;
        exisitngBook.ShortDescription = request.ShortDescription;
        exisitngBook.PublishDate = request.PublishDate;
        exisitngBook.Authors = request.Authors;

        await _repository.UpdateAsync(exisitngBook, ct);
        await _repository.SaveChangesAsync(ct);

        var updatedSnapshot = new BookSnapshot(
                          exisitngBook.Id,
                          exisitngBook.Title,
                          exisitngBook.Authors
                        );
        
        await PublishWithFallbackAsync(new BookUpdatedEvent(oldSnapshot,updatedSnapshot), ct);

        _logger.LogInformation("Updated book {BookId}", id);
        return MapToResponseModel(exisitngBook);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogInformation("Deleting book {BookId}", id);

        var book = await _repository.GetByIdAsync(id, ct);
        if (book is null)
        {
            _logger.LogWarning("Book {BookId} not found for deletion", id);
            return false;
        }

        await _repository.DeleteAsync(book);
        await _repository.SaveChangesAsync(ct);

        //await _eventBus.PublishAsync(new BookDeletedEvent(book.Id), ct);

        _logger.LogInformation("Deleted book {BookId}", id);
        return true;
    }

    private static BookResponseModel MapToResponseModel(BookManagement.API.Modules.Books.Domain.Entities.Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        ShortDescription = book.ShortDescription,
        PublishDate = book.PublishDate,
        Authors = book.Authors
    };

    private async Task PublishWithFallbackAsync<TEvent>(TEvent @event, CancellationToken ct)
        where TEvent : IEvent
    {
        try
        {
            await _eventBus.PublishAsync(@event, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to publish integration event {EventType}", typeof(TEvent).Name);

            var failed = new FailedEvent
            {
                EventType = typeof(TEvent).Name,
                Payload = JsonSerializer.Serialize(@event),
                Error = ex.ToString()
            };

            await _failedEventStore.StoreAsync(
                failed.EventType,
                failed.Payload,
                failed.Error,
                ct);
        }
    }

}
