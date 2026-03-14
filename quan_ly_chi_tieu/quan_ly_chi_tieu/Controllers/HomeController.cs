using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quan_ly_chi_tieu.Data;
using System.Security.Claims;

namespace quan_ly_chi_tieu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity?.IsAuthenticated == true)
                return View();

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var workspace = await _context.Workspaces
                .FirstOrDefaultAsync(w => w.OwnerId == userId && w.WorkspaceType == "PERSONAL");

            if (workspace == null) return View();

            var workspaceId = workspace.Id;
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            // Total assets
            var totalAssets = await _context.Wallets
                .Where(w => w.WorkspaceId == workspaceId && w.IsActive == true && w.IsIncludedInTotal == true)
                .SumAsync(w => w.CurrentBalance ?? 0);

            // This month income
            var monthIncome = await _context.Transactions
                .Where(t => t.WorkspaceId == workspaceId
                         && t.TransactionDate >= monthStart
                         && t.Category != null && t.Category.Type == "INCOME")
                .SumAsync(t => t.Amount);

            // This month expense
            var monthExpense = await _context.Transactions
                .Where(t => t.WorkspaceId == workspaceId
                         && t.TransactionDate >= monthStart
                         && t.Category != null && t.Category.Type == "EXPENSE")
                .SumAsync(t => t.Amount);

            // Recent 5 transactions
            var recentTransactions = await _context.Transactions
                .Where(t => t.WorkspaceId == workspaceId)
                .Include(t => t.Category)
                .Include(t => t.Wallet)
                .OrderByDescending(t => t.TransactionDate)
                .ThenByDescending(t => t.CreatedAt)
                .Take(5)
                .ToListAsync();

            // Wallets
            var wallets = await _context.Wallets
                .Where(w => w.WorkspaceId == workspaceId && w.IsActive == true)
                .ToListAsync();

            // Budgets with progress
            var budgets = await _context.Budgets
                .Where(b => b.WorkspaceId == workspaceId)
                .Include(b => b.Category)
                .ToListAsync();

            // Spend per category this month
            var spendByCategory = await _context.Transactions
                .Where(t => t.WorkspaceId == workspaceId
                         && t.TransactionDate >= monthStart
                         && t.Category != null && t.Category.Type == "EXPENSE")
                .GroupBy(t => new { t.CategoryId, t.Category!.Name, t.Category!.Icon, t.Category!.Color })
                .Select(g => new
                {
                    CategoryName = g.Key.Name,
                    CategoryIcon = g.Key.Icon,
                    CategoryColor = g.Key.Color,
                    Total = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .ToListAsync();

            // Saving goals
            var savingGoals = await _context.SavingGoals
                .Where(g => g.WorkspaceId == workspaceId && g.Status == "ACTIVE")
                .ToListAsync();

            ViewBag.TotalAssets = totalAssets;
            ViewBag.MonthIncome = monthIncome;
            ViewBag.MonthExpense = monthExpense;
            ViewBag.MonthBalance = monthIncome - monthExpense;
            ViewBag.RecentTransactions = recentTransactions;
            ViewBag.Wallets = wallets;
            ViewBag.Budgets = budgets;
            ViewBag.SpendByCategory = spendByCategory;
            ViewBag.SavingGoals = savingGoals;
            ViewBag.MonthLabel = now.ToString("MMMM yyyy", new System.Globalization.CultureInfo("vi-VN"));

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}