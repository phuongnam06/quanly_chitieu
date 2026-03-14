using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quan_ly_chi_tieu.Data;
using quan_ly_chi_tieu.Models;
using quan_ly_chi_tieu.Models.ViewModels;
using System.Security.Claims;

namespace quan_ly_chi_tieu.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Guid GetCurrentUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private async Task<Guid> GetWorkspaceId()
        {
            var userId = GetCurrentUserId();
            var ws = await _context.Workspaces
                .Where(w => w.OwnerId == userId && w.WorkspaceType == "PERSONAL")
                .FirstOrDefaultAsync();
            return ws?.Id ?? Guid.Empty;
        }

        // GET: /Budgets
        public async Task<IActionResult> Index()
        {
            var workspaceId = await GetWorkspaceId();
            var now = DateTime.Now;
            DateTime periodStart;
            // Default: current month
            periodStart = new DateTime(now.Year, now.Month, 1);

            var budgets = await _context.Budgets
                .Where(b => b.WorkspaceId == workspaceId)
                .Include(b => b.Category)
                .ToListAsync();

            var viewModels = new List<BudgetViewModel>();
            foreach (var b in budgets)
            {
                // Calculate spent amount in current period
                var spent = await _context.Transactions
                    .Where(t => t.WorkspaceId == workspaceId
                             && t.CategoryId == b.CategoryId
                             && t.TransactionDate >= periodStart
                             && t.TransactionDate < now.AddDays(1))
                    .SumAsync(t => t.Amount);

                viewModels.Add(new BudgetViewModel
                {
                    Id = b.Id,
                    CategoryId = b.CategoryId ?? Guid.Empty,
                    CategoryName = b.Category?.Name,
                    CategoryIcon = b.Category?.Icon,
                    CategoryColor = b.Category?.Color,
                    Amount = b.Amount,
                    SpentAmount = spent,
                    PeriodType = b.PeriodType ?? "MONTHLY",
                    AlertThreshold = b.AlertThreshold ?? 80,
                    IsRollover = b.IsRollover ?? false
                });
            }

            ViewBag.PeriodLabel = now.ToString("MMMM yyyy", new System.Globalization.CultureInfo("vi-VN"));
            return View(viewModels);
        }

        // GET: /Budgets/Create
        public async Task<IActionResult> Create()
        {
            var workspaceId = await GetWorkspaceId();
            return View(new BudgetViewModel
            {
                Categories = await _context.Categories
                    .Where(c => c.WorkspaceId == workspaceId && c.Type == "EXPENSE" && c.IsActive == true)
                    .ToListAsync()
            });
        }

        // POST: /Budgets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BudgetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var budget = new Budget
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    CategoryId = model.CategoryId,
                    Amount = model.Amount,
                    PeriodType = model.PeriodType,
                    AlertThreshold = model.AlertThreshold,
                    IsRollover = model.IsRollover,
                    CreatedAt = DateTime.Now
                };
                _context.Budgets.Add(budget);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ngân sách đã được tạo!";
                return RedirectToAction(nameof(Index));
            }
            var wsId = await GetWorkspaceId();
            model.Categories = await _context.Categories
                .Where(c => c.WorkspaceId == wsId && c.Type == "EXPENSE" && c.IsActive == true)
                .ToListAsync();
            return View(model);
        }

        // POST: /Budgets/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Id == id && b.WorkspaceId == workspaceId);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ngân sách đã được xóa!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
