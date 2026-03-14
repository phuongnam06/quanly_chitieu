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
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
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

        // GET: /Transactions
        public async Task<IActionResult> Index(TransactionFilterViewModel? filter)
        {
            var workspaceId = await GetWorkspaceId();

            var query = _context.Transactions
                .Where(t => t.WorkspaceId == workspaceId)
                .Include(t => t.Category)
                .Include(t => t.Wallet)
                .Include(t => t.Attachments)
                .AsQueryable();

            // Apply filters
            filter ??= new TransactionFilterViewModel();
            if (filter.FromDate.HasValue)
                query = query.Where(t => t.TransactionDate >= filter.FromDate.Value);
            if (filter.ToDate.HasValue)
                query = query.Where(t => t.TransactionDate <= filter.ToDate.Value.AddDays(1));
            if (filter.WalletId.HasValue)
                query = query.Where(t => t.WalletId == filter.WalletId.Value);
            if (filter.CategoryId.HasValue)
                query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
            if (!string.IsNullOrEmpty(filter.Type) && filter.Type != "all")
                query = query.Where(t => t.Category!.Type == filter.Type);
            if (!string.IsNullOrEmpty(filter.Keyword))
                query = query.Where(t => (t.Note != null && t.Note.Contains(filter.Keyword)) 
                                      || (t.Payee != null && t.Payee.Contains(filter.Keyword)));

            var transactions = await query
                .OrderByDescending(t => t.TransactionDate)
                .ThenByDescending(t => t.CreatedAt)
                .Take(200)
                .ToListAsync();

            // Totals
            ViewBag.TotalIncome = transactions
                .Where(t => t.Category?.Type == "INCOME").Sum(t => t.Amount);
            ViewBag.TotalExpense = transactions
                .Where(t => t.Category?.Type == "EXPENSE").Sum(t => t.Amount);

            // For filter dropdowns
            ViewBag.Wallets = await _context.Wallets
                .Where(w => w.WorkspaceId == workspaceId && w.IsActive == true).ToListAsync();
            ViewBag.Categories = await _context.Categories
                .Where(c => c.WorkspaceId == workspaceId && c.IsActive == true).ToListAsync();
            ViewBag.Filter = filter;

            return View(transactions);
        }

        // GET: /Transactions/Create
        public async Task<IActionResult> Create(string type = "EXPENSE")
        {
            var workspaceId = await GetWorkspaceId();
            var model = new TransactionViewModel
            {
                Type = type,
                TransactionDate = DateTime.Today,
                Categories = await _context.Categories
                    .Where(c => c.WorkspaceId == workspaceId && c.IsActive == true)
                    .OrderBy(c => c.Type).ThenBy(c => c.Name).ToListAsync(),
                Wallets = await _context.Wallets
                    .Where(w => w.WorkspaceId == workspaceId && w.IsActive == true).ToListAsync()
            };
            return View(model);
        }

        // POST: /Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var userId = GetCurrentUserId();

                // Validate wallet & category belong to workspace
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == model.WalletId && w.WorkspaceId == workspaceId);
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == model.CategoryId && c.WorkspaceId == workspaceId);
                if (wallet == null || category == null)
                {
                    ModelState.AddModelError("", "Ví hoặc danh mục không hợp lệ.");
                    await ReloadDropdowns(model, workspaceId);
                    return View(model);
                }

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    WalletId = model.WalletId,
                    CategoryId = model.CategoryId,
                    CreatedByUserId = userId,
                    Amount = model.Amount,
                    TransactionDate = model.TransactionDate,
                    Note = model.Note,
                    Payee = model.Payee,
                    Source = "MANUAL",
                    Status = "COMPLETED",
                    CreatedAt = DateTime.Now
                };
                _context.Transactions.Add(transaction);

                // Handle Attachment
                if (model.AttachmentFile != null && model.AttachmentFile.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.AttachmentFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/transactions", fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.AttachmentFile.CopyToAsync(stream);
                    }

                    var attachment = new Attachment
                    {
                        Id = Guid.NewGuid(),
                        TransactionId = transaction.Id,
                        FileUrl = "/uploads/transactions/" + fileName,
                        FileName = model.AttachmentFile.FileName,
                        FileType = model.AttachmentFile.ContentType,
                        FileSize = (int)model.AttachmentFile.Length,
                        CreatedAt = DateTime.Now
                    };
                    _context.Attachments.Add(attachment);
                }

                // Update wallet balance
                if (category.Type == "EXPENSE")
                    wallet.CurrentBalance -= model.Amount;
                else
                    wallet.CurrentBalance += model.Amount;

                wallet.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã ghi nhận giao dịch {model.Amount:N0}đ!";
                return RedirectToAction(nameof(Index));
            }

            var wsId = await GetWorkspaceId();
            await ReloadDropdowns(model, wsId);
            return View(model);
        }

        // GET: /Transactions/Edit/id
        public async Task<IActionResult> Edit(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var t = await _context.Transactions
                .Include(x => x.Category)
                .Include(x => x.Attachments)
                .FirstOrDefaultAsync(x => x.Id == id && x.WorkspaceId == workspaceId);
            if (t == null) return NotFound();

            var model = new TransactionViewModel
            {
                Id = t.Id,
                Amount = t.Amount,
                CategoryId = t.CategoryId,
                WalletId = t.WalletId,
                TransactionDate = t.TransactionDate,
                Note = t.Note,
                Payee = t.Payee,
                Type = t.Category?.Type ?? "EXPENSE",
                Attachments = t.Attachments.ToList()
            };
            await ReloadDropdowns(model, workspaceId);
            return View(model);
        }

        // POST: /Transactions/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var t = await _context.Transactions
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id && x.WorkspaceId == workspaceId);
                if (t == null) return NotFound();

                // Revert old balance change
                var oldWallet = await _context.Wallets.FindAsync(t.WalletId);
                if (oldWallet != null)
                {
                    if (t.Category?.Type == "EXPENSE") oldWallet.CurrentBalance += t.Amount;
                    else oldWallet.CurrentBalance -= t.Amount;
                }

                // Apply new change
                var newWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == model.WalletId && w.WorkspaceId == workspaceId);
                var newCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == model.CategoryId && c.WorkspaceId == workspaceId);
                if (newWallet == null || newCategory == null)
                {
                    await ReloadDropdowns(model, workspaceId);
                    return View(model);
                }

                t.Amount = model.Amount;
                t.WalletId = model.WalletId;
                t.CategoryId = model.CategoryId;
                t.TransactionDate = model.TransactionDate;
                t.Note = model.Note;
                t.Payee = model.Payee;
                t.UpdatedAt = DateTime.Now;

                if (newCategory.Type == "EXPENSE") newWallet.CurrentBalance -= model.Amount;
                else newWallet.CurrentBalance += model.Amount;
                newWallet.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Giao dịch đã được cập nhật!";
                return RedirectToAction(nameof(Index));
            }

            var wsId = await GetWorkspaceId();
            await ReloadDropdowns(model, wsId);
            return View(model);
        }

        // POST: /Transactions/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var t = await _context.Transactions
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id && x.WorkspaceId == workspaceId);
            if (t != null)
            {
                // Revert balance
                var wallet = await _context.Wallets.FindAsync(t.WalletId);
                if (wallet != null)
                {
                    if (t.Category?.Type == "EXPENSE") wallet.CurrentBalance += t.Amount;
                    else wallet.CurrentBalance -= t.Amount;
                    wallet.UpdatedAt = DateTime.Now;
                }
                _context.Transactions.Remove(t);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Giao dịch đã được xóa và số dư đã được hoàn trả!";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task ReloadDropdowns(TransactionViewModel model, Guid workspaceId)
        {
            model.Categories = await _context.Categories
                .Where(c => c.WorkspaceId == workspaceId && c.IsActive == true)
                .OrderBy(c => c.Type).ThenBy(c => c.Name).ToListAsync();
            model.Wallets = await _context.Wallets
                .Where(w => w.WorkspaceId == workspaceId && w.IsActive == true).ToListAsync();
        }
    }
}
