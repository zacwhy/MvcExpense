using System;
using System.ComponentModel.DataAnnotations;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseEditModel
    {
        public long Id { get; set; }

        [DataType( DataType.Date )]
        public DateTime Date { get; set; }

        public int Sequence { get; set; }

        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        public string Remarks { get; set; }

        public long ConsumerId { get; set; }

        //[Required]
        public long CategoryId { get; set; }

        public long? PaymentMethodId { get; set; }
    }
}