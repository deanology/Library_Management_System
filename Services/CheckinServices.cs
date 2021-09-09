using System;
using System.Threading.Tasks;
using Library_Management_System.Models;
using Library_Management_System.Repositories;
using Library_Management_System.Response;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Services
{
    public interface ICheckinServices
    {
        Task<ResponseModel> CreateCheckout(CheckIn checkIn);
    }
    public class CheckinServices : ICheckinServices
    {
        private readonly ICheckoutServices _iCheckoutServices;
        private readonly ILogger _logger;
        public CheckinServices(ICheckoutServices checkoutServices, ILoggerFactory loggerFactory)
        {
            _iCheckoutServices = checkoutServices;
            _logger = loggerFactory.CreateLogger(GetType());
        }
        public Task<ResponseModel> CreateCheckout(CheckIn checkIn)
        {
            throw new NotImplementedException();
        }
        public async Task<object> GetCheckOutDetailsAsync(string username, string bookname)
        {
            var checkout = await _iCheckoutServices.GetAllCheckouts();
            return checkout;

        }
    }
}
