using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Repositories;
using Library_Management_System.Response;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Services
{
    public interface IBookServices
    {
        Task<ResponseModel> CreateBook(Books book);
        Task<IEnumerable<Books>> GetAllBooks();
        Task<IEnumerable<Books>> SearchBooks(Search searchTerm);
        Task<Books> GetBookByIsbn(string isbn);
    }
    public class BookServices : IBookServices
    {
        private readonly IBookRepository _iBookRepository;
        private readonly ILogger _logger;
        public BookServices(IBookRepository iBookRepository, ILoggerFactory loggerFactory)
        {
            _iBookRepository = iBookRepository;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<ResponseModel> CreateBook(Books book)
        {
            if (string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.ISBN) ||
                string.IsNullOrEmpty(book.PublishYear))
                throw new Exception("All Fields are required");
            var newBook = new Books
            {
                Title = book.Title,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                CoverPrice = book.CoverPrice,
                AvailabilityStatus = true
            };
            var createBookResponse = await _iBookRepository.CreateBook(newBook);
            if (createBookResponse)
            {
                var response = new ResponseModel();
                return response;
            }
            else
            {
                var response = new ResponseModel();
                return response;
            }
        }

        public async Task<IEnumerable<Books>> GetAllBooks()
        {
            var allBooks = await _iBookRepository.GetAll();
            if(allBooks != null)
            {
                var sortedBooks = allBooks.ToList().OrderBy(x => x.PublishYear);
                return sortedBooks;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Books>> SearchBooks(Search searchTerm)
        {
            var allBooks = await _iBookRepository.GetAll();
            if (allBooks != null)
            {
                var sortedBooks = allBooks.ToList().
                    Where(x => (x.Title.ToLower() == searchTerm.SearchTerm.ToLower() || x.ISBN.ToLower() == searchTerm.SearchTerm.ToLower()) && x.AvailabilityStatus == true);
                
                return sortedBooks;
            }
            else
            {
                return null;
            }
        }
        public async Task<Books> GetBookByIsbn(string isbn)
        {
            var allBooks = await _iBookRepository.GetAll();
            if (allBooks != null)
            {
                var sortedBooks = allBooks.ToList().Where(x => x.ISBN == isbn).FirstOrDefault();
                return sortedBooks;
            }
            else
            {
                return null;
            }
        }
    }
}
