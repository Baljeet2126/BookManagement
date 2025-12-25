using Asp.Versioning;
using BookManagement.API.Modules.History.Api.Dtos;
using BookManagement.API.Modules.History.Application.Models;
using BookManagement.API.Modules.History.Application.Services;
using BookManagement.API.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace BookManagement.API.Modules.History.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public sealed class HistoryController : ControllerBase
    {
        private readonly HistoryService _historyService;

        public HistoryController(HistoryService historyService)
        {
            _historyService = historyService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<HistoryResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookHistory(
            [FromQuery] HistoryQuery query,
            CancellationToken ct)
        {
            var result = await _historyService.GetAsync(query, ct);
            return Ok(result);
        }
    }
}
