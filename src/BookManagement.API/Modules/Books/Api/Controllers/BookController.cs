using Asp.Versioning;
using BookManagement.API.Modules.Books.Api.Dtos;
using BookManagement.API.Modules.Books.Application.Models;
using BookManagement.API.Modules.Books.Application.Services;
using BookManagement.API.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace BookManagement.API.Modules.Books.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public sealed class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<BookResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooks(
            [FromQuery] BookQuery query,
            CancellationToken ct)
        {
            var result = await _bookService.GetPagedAsync(query, ct);
            return Ok(result);
        }

        // GET api/books/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var book = await _bookService.GetByIdAsync(id, ct);
            if (book is null)
                return NotFound();

            return Ok(book);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Create([FromBody] BookRequestModel request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _bookService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = book.Id, version = "1.0" }, book);
        }

        // PUT api/books/{id}
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(BookResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookRequestModel request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedBook = await _bookService.UpdateAsync(id, request, ct);
            if (updatedBook is null)
                return NotFound();

            return Ok(updatedBook);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _bookService.DeleteAsync(id, ct);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
