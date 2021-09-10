using System;
using System.Threading.Tasks;
using Library_Management_System.Entity;
using Library_Management_System.Models;
using Microsoft.Extensions.Logging;

namespace Library_Management_System.Repositories
{
    public interface ICheckinRepository : IGenericRepository<CheckIn>
    {
        Task<bool> CreateCheckin(CheckIn checkIn);
    }
    public class CheckinRepository : GenericRepository<CheckIn>, ICheckinRepository
    {
        private readonly ILogger<CheckinRepository> _logger;
        public CheckinRepository(ApplicationDbContext context, ILogger<CheckinRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<bool> CreateCheckin(CheckIn checkIn)
        {
            try
            {
                var creatingCheckin = await Add(checkIn);
                await SaveChanges();
                return creatingCheckin;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured in creating checkin, server returned : {ex.Message}");
                return false;
            }
        }
    }
}
