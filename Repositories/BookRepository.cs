using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library_Management_System.Entity;
using Library_Management_System.Models;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Repositories
{
    public interface IBookRepository : IGenericRepository<Books>
    {
        Task<bool> CreateBook(Books book);
        Task<IEnumerable<Books>> AllBooks();
        Task<Books> GetBooksById(int id);
    }
    public class BookRepository : GenericRepository<Books>, IBookRepository
    {
        private readonly ILogger<BookRepository> _logger;
        public BookRepository(ApplicationDbContext context, ILogger<BookRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public Task<IEnumerable<Books>> AllBooks()
        {
            try
            {
                var books = GetAll();
                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in retrieving all books : {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateBook(Books book)
        {
            try
            {
                var creatingBookResult = await Add(book);
                if (creatingBookResult)
                {
                    await SaveChanges();
                    return creatingBookResult;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in creating new book, server returned : {ex.Message}");
                return false;
            }
        }

        public async Task<Books> GetBooksById(int id)
        {
            try
            {
                var book = await GetById(id);
                if(book == null)
                {
                    return null;
                }
                else
                {
                    return book;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in creating new book, server returned : {ex.Message}");
                return null;
            }
        }
    }
}
