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
    public class DebtsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DebtsController(ApplicationDbContext context)
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

        // GET: /Debts
        public async Task<IActionResult> Index()
        {
            var workspaceId = await GetWorkspaceId();
            var debts = await _context.Debts
                .Where(d => d.WorkspaceId == workspaceId)
                .Include(d => d.Wallet)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
            return View(debts);
        }

        // GET: /Debts/Create
        public async Task<IActionResult> Create()
        {
            var workspaceId = await GetWorkspaceId();
            var model = new DebtViewModel
            {
                Wallets = await _context.Wallets.Where(w => w.WorkspaceId == workspaceId && w.IsActive == true).ToListAsync()
            };
            return View(model);
        }

        // POST: /Debts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DebtViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var userId = GetCurrentUserId();
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == model.WalletId && w.WorkspaceId == workspaceId);

                if (wallet == null) return NotFound();

                // 1. Create Debt Record
                var debt = new Debt
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    WalletId = model.WalletId,
                    DebtType = model.DebtType,
                    CounterpartyName = model.CounterpartyName,
                    InitialAmount = model.InitialAmount,
                    RemainingAmount = model.InitialAmount,
                    InterestRate = model.InterestRate,
                    StartDate = DateOnly.FromDateTime(model.StartDate),
                    DueDate = model.DueDate.HasValue ? DateOnly.FromDateTime(model.DueDate.Value) : null,
                    Note = model.Note,
                    Status = "ACTIVE",
                    CreatedAt = DateTime.Now
                };

                // 2. Create Transaction to update Wallet
                // We'll look for or create a "Debt" category
                var categoryType = model.DebtType == "BORROWED" ? "INCOME" : "EXPENSE";
                var categoryIcon = model.DebtType == "BORROWED" ? "📥" : "📤";
                var categoryName = model.DebtType == "BORROWED" ? "Đi vay" : "Cho vay";

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.WorkspaceId == workspaceId && c.Name == categoryName && c.Type == categoryType);
                
                if (category == null)
                {
                    category = new Category
                    {
                        Id = Guid.NewGuid(),
                        WorkspaceId = workspaceId,
                        Name = categoryName,
                        Type = categoryType,
                        Icon = categoryIcon,
                        Color = "#607D8B",
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
                    Amount = model.InitialAmount,
                    TransactionDate = model.StartDate,
                    Note = $"[Công nợ] {model.DebtType}: {model.CounterpartyName}. {model.Note}",
                    Payee = model.CounterpartyName,
                    Status = "COMPLETED",
                    CreatedAt = DateTime.Now
                };

                // Update Wallet
                if (categoryType == "INCOME") wallet.CurrentBalance += model.InitialAmount;
                else wallet.CurrentBalance -= model.InitialAmount;
                wallet.UpdatedAt = DateTime.Now;

                _context.Debts.Add(debt);
                _context.Transactions.Add(transaction);

                await _context.SaveChangesAsync();
                
                // Link transaction to debt in DebtTransaction
                var debtTx = new DebtTransaction
                {
                    Id = Guid.NewGuid(),
                    DebtId = debt.Id,
                    TransactionId = transaction.Id,
                    ActionType = "INITIAL",
                    Amount = model.InitialAmount,
                    PrincipalAmount = model.InitialAmount,
                    InterestAmount = 0,
                    CreatedAt = DateTime.Now
                };
                _context.DebtTransactions.Add(debtTx);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Đã ghi nhận khoản {(model.DebtType == "BORROWED" ? "đi vay" : "cho vay")} {model.InitialAmount:N0}đ";
                return RedirectToAction(nameof(Index));
            }

            var wsId = await GetWorkspaceId();
            model.Wallets = await _context.Wallets.Where(w => w.WorkspaceId == wsId && w.IsActive == true).ToListAsync();
            return View(model);
        }

        // GET: /Debts/Repay/id
        public async Task<IActionResult> Repay(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var debt = await _context.Debts.FirstOrDefaultAsync(d => d.Id == id && d.WorkspaceId == workspaceId);
            if (debt == null) return NotFound();

            var model = new RepaymentViewModel
            {
                DebtId = debt.Id,
                CounterpartyName = debt.CounterpartyName,
                DebtType = debt.DebtType,
                Amount = debt.RemainingAmount,
                Wallets = await _context.Wallets.Where(w => w.WorkspaceId == workspaceId && w.IsActive == true).ToListAsync()
            };
            return View(model);
        }

        // POST: /Debts/Repay
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Repay(RepaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var userId = GetCurrentUserId();
                var debt = await _context.Debts.FirstOrDefaultAsync(d => d.Id == model.DebtId && d.WorkspaceId == workspaceId);
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == model.WalletId && w.WorkspaceId == workspaceId);

                if (debt == null || wallet == null) return NotFound();

                // Logic Trả nợ (BORROWED) -> Wallet giảm (Expense)
                // Logic Thu nợ (LENT) -> Wallet tăng (Income)
                var actionType = debt.DebtType == "BORROWED" ? "REPAYMENT" : "COLLECTION";
                var categoryType = debt.DebtType == "BORROWED" ? "EXPENSE" : "INCOME";
                var categoryName = debt.DebtType == "BORROWED" ? "Trả nợ" : "Thu nợ";

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.WorkspaceId == workspaceId && c.Name == categoryName && c.Type == categoryType);
                
                if (category == null)
                {
                    category = new Category
                    {
                        Id = Guid.NewGuid(),
                        WorkspaceId = workspaceId,
                        Name = categoryName,
                        Type = categoryType,
                        Icon = "⚖️",
                        Color = "#009688",
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
                    Amount = model.Amount,
                    TransactionDate = model.Date,
                    Note = $"[{categoryName}] {debt.CounterpartyName}. {model.Note}",
                    Payee = debt.CounterpartyName,
                    Status = "COMPLETED",
                    CreatedAt = DateTime.Now
                };

                // Update Wallet
                if (categoryType == "INCOME") wallet.CurrentBalance += model.Amount;
                else wallet.CurrentBalance -= model.Amount;
                wallet.UpdatedAt = DateTime.Now;

                // Update Debt
                debt.RemainingAmount -= model.Amount;
                if (debt.RemainingAmount <= 0)
                {
                    debt.RemainingAmount = 0;
                    debt.Status = "COMPLETED";
                }
                debt.UpdatedAt = DateTime.Now;

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                var debtTx = new DebtTransaction
                {
                    Id = Guid.NewGuid(),
                    DebtId = debt.Id,
                    TransactionId = transaction.Id,
                    ActionType = actionType,
                    Amount = model.Amount,
                    PrincipalAmount = model.Amount,
                    InterestAmount = 0,
                    CreatedAt = DateTime.Now
                };
                _context.DebtTransactions.Add(debtTx);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Đã thực hiện {categoryName} {model.Amount:N0}đ";
                return RedirectToAction(nameof(Index));
            }

            var wsId = await GetWorkspaceId();
            model.Wallets = await _context.Wallets.Where(w => w.WorkspaceId == wsId && w.IsActive == true).ToListAsync();
            return View(model);
        }
    }
}
