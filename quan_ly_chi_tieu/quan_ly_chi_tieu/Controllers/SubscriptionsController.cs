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
    public class SubscriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Guid GetCurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private async Task<Guid> GetWorkspaceId()
        {
            var userId = GetCurrentUserId();
            var ws = await _context.Workspaces.FirstOrDefaultAsync(w => w.OwnerId == userId && w.WorkspaceType == "PERSONAL");
            return ws?.Id ?? Guid.Empty;
        }

        public async Task<IActionResult> Index()
        {
            var workspaceId = await GetWorkspaceId();
            var subs = await _context.Subscriptions
                .Where(s => s.WorkspaceId == workspaceId)
                .Include(s => s.Category)
                .Include(s => s.Wallet)
                .OrderBy(s => s.NextExecutionDate)
                .ToListAsync();
            return View(subs);
        }

        public async Task<IActionResult> Create()
        {
            var workspaceId = await GetWorkspaceId();
            var model = new SubscriptionViewModel
            {
                Categories = await _context.Categories.Where(c => c.WorkspaceId == workspaceId && c.IsActive == true).ToListAsync(),
                Wallets = await _context.Wallets.Where(w => w.WorkspaceId == workspaceId && w.IsActive == true).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var sub = new Subscription
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    WalletId = model.WalletId,
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    Type = model.Type,
                    Amount = model.Amount,
                    Frequency = model.Frequency,
                    StartDate = DateOnly.FromDateTime(model.StartDate),
                    NextExecutionDate = DateOnly.FromDateTime(model.StartDate),
                    IsAutoInsert = model.IsAutoInsert,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                _context.Subscriptions.Add(sub);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã tạo lịch thu chi định kỳ cho \"{model.Name}\"";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var sub = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id && s.WorkspaceId == workspaceId);
            if (sub != null)
            {
                _context.Subscriptions.Remove(sub);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã hủy lịch thu chi định kỳ.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
