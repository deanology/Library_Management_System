using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Response;
using Library_Management_System.Services;
using Library_Management_System.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library_Management_System.Controllers
{
    public class CheckinController : Controller
    {
        private readonly ILogger<CheckinController> _logger;
        private readonly ICheckinServices _icheckinServices;
        public CheckinController(ILogger<CheckinController> logger, ICheckinServices iCheckinServices)
        {
            _logger = logger;
            _icheckinServices = iCheckinServices;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckinAsync(CheckingEmail model)
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
                var checkinREsponse = await _icheckinServices.CreateCheckout(model);

                if (checkinREsponse == null)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = "Checkout creation not successfully"
                    });
                return StatusCode(StatusCodes.Status201Created, new { ResponseCode = ResponseCodes.Success });
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
