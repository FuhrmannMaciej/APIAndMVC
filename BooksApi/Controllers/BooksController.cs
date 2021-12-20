using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksApi.Models;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using BooksApi.Attributes;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiKey]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation("Zwraca wszystkie książki.", "Używa EF ToListAsync()")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Zwraca książkę o podanym {id}.", "Używa EF FindAsync()")]
        [SwaggerResponse(404, "Nie znaleziono książki o podanym {id}")]
        public async Task<ActionResult<Book>> GetBook(
            [SwaggerParameter("Podaj {id} książki o której chcesz się więcej dowiedzieć", Required = true)] long id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation("Modyfikuje książkę o podanym {id}.")]
        [SwaggerResponse(404, "Nie znaleziono książki o podanym {id}")]
        [SwaggerResponse(400, "Nie poprawne {id}.")]
        public async Task<IActionResult> PutBook(
            [SwaggerParameter("Podaj {id} książki którą chcesz modyfikować", Required = true)] long id,
            [SwaggerParameter("Podaj parametry książki którą chcesz modyfikować", Required = true)] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerOperation("Dodaje nową książkę.")]
        public async Task<ActionResult<Book>> PostBook(
            [SwaggerParameter("Uzupełnij parametry książki którą chcesz dodać.", Required = true)] Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Usuwa książkę o podanym {id}.")]
        [SwaggerResponse(404, "Nie znaleziono książki o podanym {id}")]
        public async Task<IActionResult> DeleteBook(
            [SwaggerParameter("Podaj {id} książki którą chcesz usunąć", Required = true)] long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
