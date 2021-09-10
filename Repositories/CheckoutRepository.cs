using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library_Management_System.Entity;
using Library_Management_System.Models;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Repositories
{
    public interface ICheckoutRepository : IGenericRepository<CheckOut>
    {
        Task<bool> CreateCheckout(CheckOut checkOut);
        Task<IEnumerable<CheckOut>> AllCheckouts();
    }
    public class CheckoutRepository : GenericRepository<CheckOut>, ICheckoutRepository
    {
        private readonly ILogger<CheckoutRepository> _logger;
        public CheckoutRepository(ApplicationDbContext context, ILogger<CheckoutRepository> logger) : base(context)
        {
            _logger = logger;
        }
        public Task<IEnumerable<CheckOut>> AllCheckouts()
        {
            try
            {
                var checkouts = GetAll();
                return checkouts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in retrieving all checkouts : {ex.Message}");
                return null;
            }
        }
        public async Task<bool> CreateCheckout(CheckOut checkOut)
        {
            try
            {
                var creatingCheckout = await Add(checkOut);
                await SaveChanges();
                return creatingCheckout;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in creating chechout, server returned : {ex.Message}");
                return false;
            }
        }
    }
}
