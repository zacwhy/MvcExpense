using System;
using System.ComponentModel.DataAnnotations;

namespace MvcExpense.ViewModels
{
    public class TestEditorViewModel
    {
        [DataType( DataType.Date )]
        public DateTime SampleDate { get; set; }

        [DataType( DataType.Date )]
        public DateTime SampleDate2 { get; set; }
    }
}