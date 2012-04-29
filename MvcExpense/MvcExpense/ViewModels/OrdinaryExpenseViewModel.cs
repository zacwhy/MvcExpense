using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Models;
using System.Collections;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseViewModel
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
        public long CategoryId { get; set; }
        public long? PaymentMethodId { get; set; }

        public long[] SelectedConsumerIds { get; set; }

        public MultiSelectList Consumers
        {
            get
            {
                MultiSelectList multiSelectList = GetConsumers( SelectedConsumerIds );
                return multiSelectList;
            }
        }

        public IEnumerable ConsumerList { get; set; }

        private MultiSelectList GetConsumers( long[] selectedValues )
        {
            //var db = new zExpenseEntities();
            //List<Consumer> consumers = db.Consumers.ToList();
            var multiSelectList = new MultiSelectList( ConsumerList, "Id", "Name", selectedValues );
            return multiSelectList;
        }

    }
}