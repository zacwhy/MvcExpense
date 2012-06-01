using System;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseViewModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int Sequence { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string ConsumerName { get; set; }
        public string CategoryName { get; set; }
        public string PaymentMethodName { get; set; }
    }
}