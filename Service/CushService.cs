using cush.Data;
using cush.DTO;
using cush.Models;
using Microsoft.EntityFrameworkCore;

namespace cush.Service
{
    public class CushService
    {
        private readonly ApiDbContext _context;
        private readonly ILogger _logger;

        public CushService(ApiDbContext context, ILogger<CushService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateCushesAsync(CushDTO cushDto)
        {
            try
            {
                var transaction = new Cush
                {
                    Amount = cushDto.Amount,
                    Type = cushDto.Type,
                    Kind = cushDto.Kind,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Cushes.Add(transaction);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }
        public async Task<ResDto> GetCushesInTimeRange(DateTime startDate, DateTime endDate)
        {
            var transactions = await _context.Cushes
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .ToListAsync();

            var totalTransactions = transactions.Count;
            var totalIncome = transactions.Where(t => t.Type == 1).Sum(t => t.Amount);
            var totalOutcome = transactions.Where(t => t.Type == 0).Sum(t => t.Amount);
            var totalsAmount = totalIncome - totalOutcome;
            var result = new ResDto{
                    Total = totalTransactions,
                    Income = totalIncome,
                    Outcome = totalOutcome,
                    Amount = totalsAmount
                };
            return result;
        }
    }
}