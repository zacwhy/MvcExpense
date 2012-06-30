using System;
using System.ComponentModel.DataAnnotations;

namespace MvcExpense.UI.Models.Input
{
    public class EditOrdinaryExpenseInput
    {
        //[Required] // implicit
        public long Id { get; set; }

        [Required] // todo make this work
        [DataType( DataType.Date )]
        public DateTime Date { get; set; }

        [Required( ErrorMessage = "The Price field is required." )]
        public double Price { get; set; }

        [Required( ErrorMessage = "The Sequence field is required." )]
        public int Sequence { get; set; }

        [Required]
        public string Description { get; set; }

        public string Remarks { get; set; }

        [Required( ErrorMessage = "The Category field is required." )] // todo make this work
        public long CategoryId { get; set; }

        [Required( ErrorMessage = "The Consumer field is required." )]
        public long ConsumerId { get; set; }

        public long? PaymentMethodId { get; set; }
    }
}