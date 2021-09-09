using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Services;
using Library_Management_System.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserServices _iUserService;

        public UserController(ILogger<UserController> logger, IUserServices iUserService)
        {
            _logger = logger;
            _iUserService = iUserService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("registeruser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = "Incomplete Parameters"
                    });
                var user = await _iUserService.CreateAsync(model);
                return Ok(new
                {
                    ResponseCode = StatusCodes.Status201Created,
                    ResponseObject = new
                    {
                        user
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ResponseCode = ResponseCodes.UnexpectedError,
                    ResponseDescription = $"An unexpected error occured. Please try again!, {ex.Message}"
                });
            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterModel model)
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

                //sign in user
                var user = await _iUserService.AuthAsync(model);

                if (user == null)
                    return Unauthorized(new
                    {
                        ResponseCode = ResponseCodes.Unauthorized,
                        ResponseDescription = "Authentication failed. Check your credentials and try again!"
                    });

                return Ok(new
                {
                    ResponseCode = ResponseCodes.Success,
                    ResponseObject = user
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ResponseCode = ResponseCodes.UnexpectedError,
                    ResponseDescription = $"An unexpected error occured. Please try again!, {ex.Message}"
                });
            }
        }
    }
}
