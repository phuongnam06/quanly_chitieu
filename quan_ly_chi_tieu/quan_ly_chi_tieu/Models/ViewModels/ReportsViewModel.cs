using System;
using System.Collections.Generic;

namespace quan_ly_chi_tieu.Models.ViewModels
{
    public class ReportsViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetBalance { get; set; }

        public List<CategorySpending>? SpendingByCategory { get; set; }
        public List<DailySpending>? DailyTrends { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CategorySpending
    {
        public string? CategoryName { get; set; }
        public string? Color { get; set; }
        public decimal Amount { get; set; }
        public double Percentage { get; set; }
    }

    public class DailySpending
    {
        public string? DateLabel { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
