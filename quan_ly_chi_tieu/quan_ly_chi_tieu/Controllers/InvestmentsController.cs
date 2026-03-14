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
    public class InvestmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvestmentsController(ApplicationDbContext context)
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
            var assets = await _context.InvestmentAssets
                .Where(a => a.WorkspaceId == workspaceId && a.IsActive == true)
                .Include(a => a.InvestmentTransactions)
                .ToListAsync();
            
            ViewBag.Wallets = await _context.Wallets.Where(w => w.WorkspaceId == workspaceId && w.IsActive == true).ToListAsync();
            return View(assets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsset(AssetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var asset = new InvestmentAsset
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    AssetType = model.AssetType,
                    Symbol = model.Symbol.ToUpper(),
                    Name = model.Name,
                    CurrentPrice = model.CurrentPrice,
                    IsActive = true
                };
                _context.InvestmentAssets.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessTransaction(InvestmentTxViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var userId = GetCurrentUserId();
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == model.WalletId && w.WorkspaceId == workspaceId);
                var asset = await _context.InvestmentAssets.FirstOrDefaultAsync(a => a.Id == model.AssetId && a.WorkspaceId == workspaceId);

                if (wallet == null || asset == null) return NotFound();

                decimal totalValue = model.Quantity * model.Price;

                // 1. Create Investment Transaction
                var invTx = new InvestmentTransaction
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    AssetId = model.AssetId,
                    WalletId = model.WalletId,
                    ActionType = model.TransactionType, // BUY, SELL, DIVIDEND
                    Quantity = model.Quantity,
                    UnitPrice = model.Price,
                    TransactionDate = model.Date,
                    CreatedAt = DateTime.Now
                };

                // 2. Create Wallet Transaction to update balance
                var isIncome = model.TransactionType == "SELL" || model.TransactionType == "DIVIDEND";
                var categoryName = $"Đầu tư ({model.TransactionType})";
                var categoryType = isIncome ? "INCOME" : "EXPENSE";

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.WorkspaceId == workspaceId && c.Name == categoryName);
                
                if (category == null)
                {
                    category = new Category
                    {
                        Id = Guid.NewGuid(),
                        WorkspaceId = workspaceId,
                        Name = categoryName,
                        Type = categoryType,
                        Icon = "📈",
                        Color = "#2196F3",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };
                    _context.Categories.Add(category);
                }

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    WalletId = model.WalletId,
                    CategoryId = category.Id,
                    CreatedByUserId = userId,
                    Amount = totalValue,
                    TransactionDate = model.Date,
                    Note = $"[Đầu tư] {model.TransactionType} {model.Quantity} {asset.Symbol} @ {model.Price:N0}",
                    Status = "COMPLETED",
                    CreatedAt = DateTime.Now
                };

                // Update Wallet
                if (isIncome) wallet.CurrentBalance += totalValue;
                else wallet.CurrentBalance -= totalValue;
                wallet.UpdatedAt = DateTime.Now;

                _context.InvestmentTransactions.Add(invTx);
                _context.Transactions.Add(transaction);
                
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã ghi nhận giao dịch {model.TransactionType} {asset.Symbol}";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
