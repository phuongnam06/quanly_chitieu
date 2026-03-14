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
    public class WalletsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WalletsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Guid GetCurrentUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        private async Task<Guid> GetWorkspaceId()
        {
            var userId = GetCurrentUserId();
            var ws = await _context.Workspaces
                .Where(w => w.OwnerId == userId && w.WorkspaceType == "PERSONAL")
                .FirstOrDefaultAsync();
            return ws?.Id ?? Guid.Empty;
        }

        // GET: /Wallets
        public async Task<IActionResult> Index()
        {
            var workspaceId = await GetWorkspaceId();
            var wallets = await _context.Wallets
                .Where(w => w.WorkspaceId == workspaceId && w.IsActive == true)
                .OrderBy(w => w.Name)
                .ToListAsync();
            return View(wallets);
        }

        // GET: /Wallets/Create
        public IActionResult Create()
        {
            return View(new WalletViewModel());
        }

        // POST: /Wallets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WalletViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    Name = model.Name,
                    WalletType = model.WalletType,
                    InitialBalance = model.InitialBalance,
                    CurrentBalance = model.InitialBalance,
                    Icon = model.Icon,
                    Color = model.Color,
                    IsIncludedInTotal = model.IsIncludedInTotal,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Ví \"{wallet.Name}\" đã được tạo thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /Wallets/Edit/id
        public async Task<IActionResult> Edit(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id && w.WorkspaceId == workspaceId);
            if (wallet == null) return NotFound();

            var model = new WalletViewModel
            {
                Id = wallet.Id,
                Name = wallet.Name ?? string.Empty,
                WalletType = wallet.WalletType ?? "CASH",
                InitialBalance = wallet.InitialBalance ?? 0,
                Icon = wallet.Icon ?? "💰",
                Color = wallet.Color ?? "#4CAF50",
                IsIncludedInTotal = wallet.IsIncludedInTotal ?? true
            };
            return View(model);
        }

        // POST: /Wallets/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, WalletViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id && w.WorkspaceId == workspaceId);
                if (wallet == null) return NotFound();

                wallet.Name = model.Name;
                wallet.WalletType = model.WalletType;
                wallet.Icon = model.Icon;
                wallet.Color = model.Color;
                wallet.IsIncludedInTotal = model.IsIncludedInTotal;
                wallet.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["Success"] = $"Ví \"{wallet.Name}\" đã được cập nhật!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: /Wallets/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id && w.WorkspaceId == workspaceId);
            if (wallet != null)
            {
                wallet.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Ví \"{wallet.Name}\" đã được xóa!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
