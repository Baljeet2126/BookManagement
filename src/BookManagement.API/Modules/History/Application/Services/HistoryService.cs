using BookManagement.API.Modules.History.Api.Dtos;
using BookManagement.API.Modules.History.Application.Interfaces;
using BookManagement.API.Modules.History.Application.Models;
using BookManagement.API.Shared.Events;
using BookManagement.API.Shared.Models;

namespace BookManagement.API.Modules.History.Application.Services;

public sealed class HistoryService : IHistoryService
{
    private readonly IBookHistoryRepository _repository;
    private readonly ILogger<HistoryService> _logger;

    public HistoryService(
        IBookHistoryRepository repository,
        IEventBus eventBus,
        ILogger<HistoryService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PagedResult<HistoryResponseModel>> GetAsync(
    HistoryQuery query,
    CancellationToken ct)
    {
        var entries = await _repository.GetPagedAsync(query, ct);

        return new PagedResult<HistoryResponseModel>(
            entries.Items.Select(e => new HistoryResponseModel(
                e.Id,
                e.BookId,
                e.Action,
                e.BookTitle,
                e.Authors,
                e.Description,
                e.OccurredOn
            )).ToList(),
            entries.TotalCount,
            query.PageNumber,
            query.PageSize
        );
    }
}
