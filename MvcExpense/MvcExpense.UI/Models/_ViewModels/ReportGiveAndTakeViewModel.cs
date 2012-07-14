using System.Collections.Generic;
using MvcExpense.HelperModels;

namespace MvcExpense.ViewModels
{
    public class ReportGiveAndTakeViewModel
    {
        private IList<GiveAndTakeRecord> _records;
        public IList<GiveAndTakeRecord> Records
        {
            get
            {
                if ( _records == null )
                {
                    _records = new List<GiveAndTakeRecord>();
                }
                return _records;
            }
        }
    }
}