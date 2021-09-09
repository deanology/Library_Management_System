using System;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Repositories;
using Library_Management_System.Response;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Services
{
    public interface ICheckoutServices
    {
        Task<ResponseModel> CreateCheckout(CheckOut checkout);
        Task<ResponseModel> GetAllCheckouts();
    }
    public class CheckoutServices : ICheckoutServices
    {
        private readonly ICheckoutRepository _iCheckoutRepository;
        private readonly ILogger _logger;
        public CheckoutServices(ICheckoutRepository iCheckoutRepository, ILoggerFactory loggerFactory)
        {
            _iCheckoutRepository = iCheckoutRepository;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<ResponseModel> CreateCheckout(CheckOut checkout)
        {
            if (string.IsNullOrEmpty(checkout.Fullname) || string.IsNullOrEmpty(checkout.Email) ||
                string.IsNullOrEmpty(checkout.PhoneNumber) || string.IsNullOrEmpty(checkout.NIN))
                throw new Exception("All Fields are required");

            var checkComplete = new CheckOut
            {
                Fullname = checkout.Fullname,
                Email = checkout.Email,
                NIN = checkout.NIN,
                PhoneNumber = checkout.PhoneNumber,
                BookId = checkout.BookId,
                CheckOutDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.AddDays(14)
            };
            var createBookResponse = await _iCheckoutRepository.CreateCheckout(checkComplete);
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
        public async Task<ResponseModel> GetAllCheckouts()
        {
            var allCheckouts = await _iCheckoutRepository.AllCheckouts();
            if (allCheckouts != null)
            {
                var response = new ResponseModel();
                response.ResponseObject = allCheckouts;        
                return response;
            }
            else
            {
                var response = new ResponseModel();
                return response;
            }
        }
    }
}
