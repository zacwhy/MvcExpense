using System.Collections.Generic;
using MvcExpense.Core.Models;

namespace MvcExpense.ViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<OrdinaryExpense> OrdinaryExpenses { get; set; }
    }
}