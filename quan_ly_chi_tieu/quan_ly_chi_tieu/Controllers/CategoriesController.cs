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
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
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

        // GET: /Categories
        public async Task<IActionResult> Index()
        {
            var workspaceId = await GetWorkspaceId();
            var categories = await _context.Categories
                .Where(c => c.WorkspaceId == workspaceId && c.IsActive == true)
                .Include(c => c.Parent)
                .OrderBy(c => c.Type).ThenBy(c => c.Name)
                .ToListAsync();
            return View(categories);
        }

        // GET: /Categories/Create
        public async Task<IActionResult> Create()
        {
            var workspaceId = await GetWorkspaceId();
            var model = new CategoryViewModel
            {
                ParentCategories = await _context.Categories
                    .Where(c => c.WorkspaceId == workspaceId && c.ParentId == null && c.IsActive == true)
                    .ToListAsync()
            };
            return View(model);
        }

        // POST: /Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    WorkspaceId = workspaceId,
                    Name = model.Name,
                    Type = model.Type,
                    Icon = model.Icon,
                    Color = model.Color,
                    ParentId = model.ParentId,
                    IsActive = true,
                    IsSystem = false,
                    CreatedAt = DateTime.Now
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Danh mục \"{category.Name}\" đã được tạo!";
                return RedirectToAction(nameof(Index));
            }
            var wsId = await GetWorkspaceId();
            model.ParentCategories = await _context.Categories
                .Where(c => c.WorkspaceId == wsId && c.ParentId == null && c.IsActive == true)
                .ToListAsync();
            return View(model);
        }

        // GET: /Categories/Edit/id
        public async Task<IActionResult> Edit(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var cat = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.WorkspaceId == workspaceId);
            if (cat == null) return NotFound();
            var model = new CategoryViewModel
            {
                Id = cat.Id,
                Name = cat.Name ?? string.Empty,
                Type = cat.Type ?? "EXPENSE",
                Icon = cat.Icon ?? "📁",
                Color = cat.Color ?? "#2196F3",
                ParentId = cat.ParentId,
                ParentCategories = await _context.Categories
                    .Where(c => c.WorkspaceId == workspaceId && c.ParentId == null && c.IsActive == true && c.Id != id)
                    .ToListAsync()
            };
            return View(model);
        }

        // POST: /Categories/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workspaceId = await GetWorkspaceId();
                var cat = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.WorkspaceId == workspaceId);
                if (cat == null) return NotFound();
                cat.Name = model.Name;
                cat.Type = model.Type;
                cat.Icon = model.Icon;
                cat.Color = model.Color;
                cat.ParentId = model.ParentId;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Danh mục \"{cat.Name}\" đã được cập nhật!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: /Categories/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workspaceId = await GetWorkspaceId();
            var cat = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.WorkspaceId == workspaceId);
            if (cat != null)
            {
                cat.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Danh mục \"{cat.Name}\" đã được xóa!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
