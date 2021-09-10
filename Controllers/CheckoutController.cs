using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Request;
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
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly IBookServices _ibookServices;
        private readonly ICheckoutServices _icheckoutServices;
        public CheckoutController(ILogger<CheckoutController> logger, IBookServices iBookServices, ICheckoutServices iCheckoutServices)
        {
            _logger = logger;
            _ibookServices = iBookServices;
            _icheckoutServices = iCheckoutServices;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("addcheckout")]
        public async Task<IActionResult> CheckoutAsync(CheckoutPayload checkoutPayload)
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
                var book = await _ibookServices.GetBookByIsbn(checkoutPayload.BookISBN);
                if(book == null)
                {
                    return BadRequest(new ServiceResponse(ResponseCodes.UnexpectedError,
                        "Book not available"));
                }
                var bookId = book.Id;
                var checkoutDetails = new CheckOut
                {
                    PhoneNumber = checkoutPayload.PhoneNumber,
                    Fullname = checkoutPayload.Fullname,
                    NIN = checkoutPayload.NIN,
                    Email = checkoutPayload.Email,
                    BookId = bookId
                };
                var addCheckoutResponse = _icheckoutServices.CreateCheckout(checkoutDetails);
                if (addCheckoutResponse == null)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = "Checkout Creation not successfully"
                    });
                return StatusCode(StatusCodes.Status201Created, new { ResponseCode = ResponseCodes.Success, addCheckoutResponse });
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
