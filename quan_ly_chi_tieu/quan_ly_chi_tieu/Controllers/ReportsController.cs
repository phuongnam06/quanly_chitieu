using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quan_ly_chi_tieu.Data;
using quan_ly_chi_tieu.Models.ViewModels;
using System.Security.Claims;

namespace quan_ly_chi_tieu.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Guid GetWorkspaceId()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return _context.Workspaces
                .Where(w => w.OwnerId == userId && w.WorkspaceType == "PERSONAL")
                .Select(w => w.Id)
                .FirstOrDefault();
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var workspaceId = GetWorkspaceId();
            var end = endDate ?? DateTime.Today;
            var start = startDate ?? new DateTime(end.Year, end.Month, 1);

            var transactions = await _context.Transactions
                .Where(t => t.WorkspaceId == workspaceId && t.TransactionDate >= start && t.TransactionDate <= end)
                .Include(t => t.Category)
                .ToListAsync();

            var totalIncome = transactions.Where(t => t.Category.Type == "INCOME").Sum(t => t.Amount);
            var totalExpense = transactions.Where(t => t.Category.Type == "EXPENSE").Sum(t => t.Amount);

            // Spending by Category
            var categorySpending = transactions
                .Where(t => t.Category.Type == "EXPENSE")
                .GroupBy(t => new { t.Category.Name, t.Category.Color })
                .Select(g => new CategorySpending
                {
                    CategoryName = g.Key.Name,
                    Color = g.Key.Color,
                    Amount = g.Sum(t => t.Amount),
                    Percentage = totalExpense > 0 ? (double)(g.Sum(t => t.Amount) / totalExpense * 100) : 0
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            // Daily Trends
            var dailyTrends = transactions
                .GroupBy(t => t.TransactionDate.Date)
                .Select(g => new DailySpending
                {
                    DateLabel = g.Key.ToString("dd/MM"),
                    Income = g.Where(t => t.Category.Type == "INCOME").Sum(t => t.Amount),
                    Expense = g.Where(t => t.Category.Type == "EXPENSE").Sum(t => t.Amount)
                })
                .OrderBy(x => x.DateLabel)
                .ToList();

            var model = new ReportsViewModel
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                NetBalance = totalIncome - totalExpense,
                SpendingByCategory = categorySpending,
                DailyTrends = dailyTrends,
                StartDate = start,
                EndDate = end
            };

            return View(model);
        }
    }
}
