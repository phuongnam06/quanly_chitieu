using System.ComponentModel.DataAnnotations;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn loại")]
        [Display(Name = "Loại")]
        public string Type { get; set; } = "EXPENSE"; // INCOME or EXPENSE

        [Display(Name = "Biểu tượng")]
        public string Icon { get; set; } = "📁";

        [Display(Name = "Màu sắc")]
        public string Color { get; set; } = "#2196F3";

        [Display(Name = "Danh mục cha")]
        public Guid? ParentId { get; set; }

        public List<Category>? ParentCategories { get; set; }
    }
}
