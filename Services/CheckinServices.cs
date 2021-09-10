using System;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Repositories;
using Library_Management_System.Response;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Services
{
    public interface ICheckinServices
    {
        Task<ResponseModel> CreateCheckout(CheckingEmail model);
    }
    public class CheckinServices : ICheckinServices
    {
        private readonly ICheckoutServices _iCheckoutServices;
        private readonly ICheckinRepository _iCheckinRepository;
        private readonly IBookRepository _iBookRepository;
        private readonly ILogger _logger;
        public CheckinServices(ICheckoutServices checkoutServices, ILoggerFactory loggerFactory, ICheckinRepository iCheckoutRepository, IBookRepository iBookRepository)
        {
            _iCheckoutServices = checkoutServices;
            _logger = loggerFactory.CreateLogger(GetType());
            _iCheckinRepository = iCheckoutRepository;
            _iBookRepository = iBookRepository;
        }
        public async Task<ResponseModel> CreateCheckout(CheckingEmail model)
        {
            var checkout = await _iCheckoutServices.GetAllCheckouts();
            var userCheckoutHistory = checkout.ToList().Where(x => x.Email == model.Email).OrderByDescending(x => x.CheckOutDate).FirstOrDefault();
            //check if returned date has not elapsed expected return date
            if(DateTime.Now > userCheckoutHistory.ExpectedReturnDate)
            {
                var numberOfDaysDefalted = (DateTime.Now - userCheckoutHistory.ExpectedReturnDate).Days;
                var fee = numberOfDaysDefalted * 200;
                var checkInDetails = new CheckIn()
                {
                    Email = model.Email,
                    DaysDefaulted = numberOfDaysDefalted,
                    PenaltyFee = fee
                };
                var addCheckinResult = await _iCheckinRepository.CreateCheckin(checkInDetails);
                if (true)
                {
                    //make the book status available again
                    var book = await _iBookRepository.GetById(userCheckoutHistory.BookId);
                    book.AvailabilityStatus = true;
                    _iBookRepository.UpsertAsync(book);
                    await _iBookRepository.SaveChanges();

                    var response = new ResponseModel();
                    response.ResponseObject = "Checking in successfully completed";
                    return response;
                }
            }
            else
            {
                var checkInDetails = new CheckIn()
                {
                    Email = model.Email,
                    DaysDefaulted = 0,
                    PenaltyFee = 0
                };
                var addCheckinResult = await _iCheckinRepository.CreateCheckin(checkInDetails);
                if (true)
                {
                    //make the book status available again
                    var response = new ResponseModel();
                    response.ResponseObject = "Checking in successfully completed";
                    return response;
                }
                
            }
            
        }
        
    }
}
