using System;
using System.ComponentModel.DataAnnotations;

namespace MvcExpense.UI.Models.Input
{
    public class CreateOrdinaryExpenseInput
    {
        [Required] // todo make this work
        [DataType( DataType.Date )]
        public DateTime Date { get; set; }

        [Required( ErrorMessage = "The Price field is required." )]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required( ErrorMessage = "The Category field is required." )] // todo make this work
        public long CategoryId { get; set; }

        public long? PaymentMethodId { get; set; }

        [Required( ErrorMessage = "The Consumers field is required." )]
        public long[] SelectedConsumerIds { get; set; }

        public string Remarks { get; set; }
    }
}