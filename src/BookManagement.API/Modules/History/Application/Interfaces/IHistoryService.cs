using BookManagement.API.Modules.History.Api.Dtos;
using BookManagement.API.Modules.History.Application.Models;
using BookManagement.API.Shared.Models;

namespace BookManagement.API.Modules.History.Application.Interfaces
{
    public interface IHistoryService
    {
        Task<PagedResult<HistoryResponseModel>> GetAsync(HistoryQuery query, CancellationToken ct);
    }
}
