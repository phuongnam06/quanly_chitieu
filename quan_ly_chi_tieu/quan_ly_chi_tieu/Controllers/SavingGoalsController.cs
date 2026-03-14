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
    public class SavingGoalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SavingGoalsController(ApplicationDbContext context)
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

        // GET: /SavingGoals
        public async Task<IActionResult> Index()
        {
            var workspaceId = await GetWorkspaceId();
            var goals = await _context.SavingGoals
                .Where(g => g.WorkspaceId == workspaceId && g.Status == "ACTIVE")
                .OrderBy(g => g.TargetDate)
                .ToListAsync();
            return View(goals);
        }

        // GET: /SavingGoals/Create
        public IActionResult Create()
        {
            return View(new SavingGoalViewModel());
        }

        // POST: /SavingGoals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavingGoalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var goal = new SavingGoal
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    Name = model.Name,
                    TargetAmount = model.TargetAmount,
                    CurrentAmount = model.CurrentAmount,
                    TargetDate = model.Deadline.HasValue ? DateOnly.FromDateTime(model.Deadline.Value) : null,
                    StartDate = DateOnly.FromDateTime(DateTime.Today),
                    Icon = model.Icon,
                    Color = model.Color,
                    Status = "ACTIVE",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.SavingGoals.Add(goal);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Mục tiêu \"{goal.Name}\" đã được tạo!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: /SavingGoals/Deposit  (nạp tiền vào mục tiêu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(Guid id, decimal amount)
        {
            var workspaceId = await GetWorkspaceId();
            var goal = await _context.SavingGoals.FirstOrDefaultAsync(g => g.Id == id && g.WorkspaceId == workspaceId);
            if (goal != null && amount > 0)
            {
                goal.CurrentAmount += amount;
                goal.UpdatedAt = DateTime.Now;
                if (goal.CurrentAmount >= goal.TargetAmount)
                {
                    goal.Status = "COMPLETED";
                    TempData["Success"] = $"🎉 Chúc mừng! Bạn đã đạt mục tiêu \"{goal.Name}\"!";
                }
                else
                {
                    TempData["Success"] = $"Đã nạp {amount:N0}đ vào mục tiêu \"{goal.Name}\"!";
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /SavingGoals/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var goal = await _context.SavingGoals.FirstOrDefaultAsync(g => g.Id == id && g.WorkspaceId == workspaceId);
            if (goal != null)
            {
                goal.Status = "CANCELLED";
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Mục tiêu \"{goal.Name}\" đã được hủy!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
