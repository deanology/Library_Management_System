using System;
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
        Task<ResponseModel> GetAllBooks();
        Task<ResponseModel> SearchBooks(string searchTerm);
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
                var response = new ResponseModel
                {
                    ResponseCode = "201",
                    ResponseDescription = "Created Successfully"
                };
                return response;
            }
            else
            {
                var response = new ResponseModel
                {
                    ResponseCode = "",
                    ResponseDescription = "Bad Request"
                };
                return response;
            }
        }

        public async Task<ResponseModel> GetAllBooks()
        {
            var allBooks = await _iBookRepository.GetAll();
            if(allBooks != null)
            {
                var sortedBooks = allBooks.ToList().OrderBy(x => x.PublishYear);
                var response = new ResponseModel
                {
                    ResponseCode = "200",
                    ResponseDescription = "Successful",
                    ResponseObject = sortedBooks
                };
                return response;
            }
            else
            {
                var response = new ResponseModel
                {
                    ResponseCode = "200",
                    ResponseDescription = ""
                };
                return response;
            }
        }

        public async Task<ResponseModel> SearchBooks(string searchTerm)
        {
            var allBooks = await _iBookRepository.GetAll();
            if (allBooks != null)
            {
                var sortedBooks = allBooks.ToList().
                    Where(x => (x.Title.Contains(searchTerm) || x.ISBN.Contains(searchTerm)) && x.AvailabilityStatus == true);
                var response = new ResponseModel
                {
                    ResponseCode = "200",
                    ResponseDescription = "Successful",
                    ResponseObject = sortedBooks
                };
                return response;
            }
            else
            {
                var response = new ResponseModel
                {
                    ResponseCode = "200",
                    ResponseDescription = ""
                };
                return response;
            }
        }
    }
}
