using Asp.Versioning;
using ImagePostsAPI.Entities;
using ImagePostsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImagePostsAPI.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Produces("application/json")]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly IBookRepository _bookRepository;

    public PostsController(ILogger<PostsController> logger, IBookRepository bookRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
    }

    // GET api/posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> Get([FromQuery] int limit = 10)
    {
        if (limit <= 0 || limit > 100) return BadRequest("The limit should been between [1-100]");

        return Ok(await _bookRepository.GetBooksAsync(limit));
    }

    // GET api/posts/5
    [HttpGet("{postId}")]
    public async Task<ActionResult<Book>> Get(Guid postId)
    {
        var result = await _bookRepository.GetByIdAsync(postId);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // POST api/posts
    [HttpPost]
    public async Task<ActionResult<Book>> Post([FromBody] Book book)
    {
        if (book == null) return ValidationProblem("Invalid input! Book not informed");

        var result = await _bookRepository.CreateAsync(book);

        if (result)
        {
            return CreatedAtAction(
                nameof(Get),
                new { id = book.Id },
                book);
        }
        else
        {
            return BadRequest("Fail to persist");
        }
    }

    // DELETE api/books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return ValidationProblem("Invalid request payload");

        var bookRetrieved = await _bookRepository.GetByIdAsync(id);

        if (bookRetrieved == null)
        {
            var errorMsg = $"Invalid input! No book found with id:{id}";
            return NotFound(errorMsg);
        }

        await _bookRepository.DeleteAsync(bookRetrieved);
        return Ok();
    }
}