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
        private readonly ICheckoutRepository _iCheckoutRepository;
        private readonly ILogger _logger;
        public CheckinServices(ICheckoutRepository iCheckoutRepository, ILoggerFactory loggerFactory)
        {
            _iCheckoutRepository = iCheckoutRepository;
            _logger = loggerFactory.CreateLogger(GetType());
        }
        public Task<ResponseModel> CreateCheckout(CheckIn checkIn)
        {
            throw new NotImplementedException();
        }
        public object GetCheckOutDetails(string username, string bookname)
        {
            var checkout = _iCheckoutRepository.GetAll();
        }
    }
}
