using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Response;
using Library_Management_System.Services;
using Library_Management_System.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookServices _ibookServices;
        public BookController(ILogger<BookController> logger, IBookServices iBookServices)
        {
            _logger = logger;
            _ibookServices = iBookServices;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return Ok();
        }
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        //[Authorize]
        [HttpPost("createbooks")]
        public async Task<IActionResult> CreateBookAsync([FromBody]Books books)
        {
            try
            {
                //check model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseMessage = "Incomplete Parameters"
                    });
                }
                var createBookResponse = await _ibookServices.CreateBook(books);

                if (createBookResponse == null)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = "Book Creation not successfully"
                    });
                return StatusCode(StatusCodes.Status201Created, new { ResponseCode = ResponseCodes.Success, createBookResponse });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return BadRequest(new ServiceResponse(ResponseCodes.UnexpectedError,
                    "An unexpected error occured. Please try again!"));
            }
            
        }
        [Authorize]
        [HttpGet("allbooks")]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            try
            {
                var allBooks = await _ibookServices.GetAllBooks();
                if (allBooks == null)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = "Cannot return all books"
                    });

                return Ok(new
                {
                    ResponseCode = ResponseCodes.Success,
                    allBooks = allBooks?.Select(x => new
                    {
                        x.Title,
                        x.ISBN,
                        x.PublishYear,
                        x.CoverPrice,
                        x.AvailabilityStatus
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return BadRequest(new ServiceResponse(ResponseCodes.UnexpectedError,
                    "An unexpected error occured. Please try again!"));
            }
           
        }
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchForBooksAsync(Search searchTerm)
        {
            try
            {
                var searchedBook = await _ibookServices.SearchBooks(searchTerm);
                if (searchedBook == null)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = "Cannot return all books"
                    });

                return Ok(new
                {
                    ResponseCode = ResponseCodes.Success,
                    searchedBook = searchedBook?.Select(x => new
                    {
                        x.Title,
                        x.ISBN,
                        x.PublishYear,
                        x.CoverPrice,
                        x.AvailabilityStatus
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return BadRequest(new ServiceResponse(ResponseCodes.UnexpectedError,
                    "An unexpected error occured. Please try again!"));
            }
            
        }
    }
}
