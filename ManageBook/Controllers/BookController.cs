using ManageBook.Data;
using ManageBook.Domain;
using ManageBook.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageBook.Controllers
{
    //https://localhost:port/api/book
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookDbContext bookDbContext;
        public BookController(BookDbContext bookDbContext)
        {
            this.bookDbContext = bookDbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var course = bookDbContext.books.ToList();
            return Ok(course);
        }

        [HttpGet]
        [Route("isbn:Guid")]
        public IActionResult GetByISBN(Guid isbn)
        {
            var book = bookDbContext.books.FirstOrDefault(x => x.ISBN == isbn);
            if (book == null)
            {
                return NotFound();
            }
            var bookDTO = new BookDTO();
            bookDTO.ISBN = book.ISBN; 
            bookDTO.Title = book.Title;
            bookDTO.NumberOfPages = book.NumberOfPages;
            bookDTO.Author = book.Author;
            bookDTO.PublicationYear = book.PublicationYear;
            return Ok(bookDTO);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] AddBookDTO dto)
        {
            var bookDomain = new Book()
            {
                Title = dto.Title,
                NumberOfPages = dto.NumberOfPages,
                Author = dto.Author,
                PublicationYear = dto.PublicationYear
            };
            bookDbContext.books.Add(bookDomain);
            bookDbContext.SaveChanges();

            var book_dto = new BookDTO()
            {
                ISBN = bookDomain.ISBN,
                Title = bookDomain.Title,
                NumberOfPages = bookDomain.NumberOfPages,
                Author = bookDomain.Author,
                PublicationYear = bookDomain.PublicationYear
            };

            return CreatedAtAction(nameof(GetByISBN), new { isbn = book_dto.ISBN }, book_dto);
        }
        [HttpPut]
        [Route("{isbn:Guid}")]
        public IActionResult Update([FromRoute] Guid isbn, [FromBody] UpdateBookDTO dto)
        {
            var bookDomain = bookDbContext.books.FirstOrDefault(x => x.ISBN == isbn);
            if (bookDomain == null)
            {
                return NotFound();
            }
            bookDomain.Title = dto.Title;
            bookDomain.NumberOfPages = dto.NumberOfPages;
            bookDomain.Author = dto.Author;
            bookDomain.PublicationYear = dto.PublicationYear;
            bookDbContext.SaveChanges();
            var updated_book_dto = new UpdateBookDTO()
            {
                Title = bookDomain.Title,
                NumberOfPages = bookDomain.NumberOfPages,
                Author = bookDomain.Author,
                PublicationYear = bookDomain.PublicationYear
            };
            return Ok(updated_book_dto);
        }
        [HttpDelete]
        [Route("{isbn:Guid}")]
        public IActionResult Delete([FromRoute] Guid isbn)
        {
            var bookDomain = bookDbContext.books.FirstOrDefault(x => x.ISBN == isbn);
            if (bookDomain == null)
            {
                return NotFound();
            }
            bookDbContext.books.Remove(bookDomain);
            bookDbContext.SaveChanges();
            var bookDTO = new BookDTO()
            {
                ISBN = bookDomain.ISBN,
                Title = bookDomain.Title,
                NumberOfPages = bookDomain.NumberOfPages,
                Author = bookDomain.Author,
                PublicationYear = bookDomain.PublicationYear
            };
            return Ok(bookDTO);
        }

    }
}
