using System.Collections.Generic;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<OrdinaryExpense> OrdinaryExpenses { get; set; }
    }
}