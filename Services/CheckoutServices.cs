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
    public interface ICheckoutServices
    {
        Task<ResponseModel> CreateCheckout(CheckOut checkout);
        Task<IEnumerable<CheckOut>> GetAllCheckouts();
    }
    public class CheckoutServices : ICheckoutServices
    {
        private readonly ICheckoutRepository _iCheckoutRepository;
        private readonly IBookRepository _iBookRepository;
        private readonly ILogger _logger;
        public CheckoutServices(ICheckoutRepository iCheckoutRepository, ILoggerFactory loggerFactory, IBookRepository iBookRepository)
        {
            _iCheckoutRepository = iCheckoutRepository;
            _iBookRepository = iBookRepository;
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
                //get book and make it unavailable
                var book = await _iBookRepository.GetById(checkout.BookId);
                book.AvailabilityStatus = false;
                _iBookRepository.UpsertAsync(book);
                await _iBookRepository.SaveChanges();

                var response = new ResponseModel();
                response.ResponseObject = "Checkout Successful";
                return response;
            }
            else
            {
                var response = new ResponseModel();
                return response;
            }
        }
        public async Task<IEnumerable<CheckOut>> GetAllCheckouts()
        {
            var allCheckout = await _iCheckoutRepository.AllCheckouts();
            if (allCheckout != null)
            {
                var checkouts = allCheckout.ToList();
                return checkouts;
            }
            else
            {
                return null;
            }
        }
    }
}
