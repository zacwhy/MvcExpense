using System.Collections.Generic;
using System.Linq;
using MvcExpense.Models;

namespace MvcExpense
{
    public static class ExpenseEntitiesCache
    {
        private static IList<Category> _categories;
        private static IList<PaymentMethod> _paymentMethods;
        private static IList<Consumer> _consumers;

        public static IList<Category> GetCategories( zExpenseEntities db )
        {
            if ( _categories == null )
            {
                RefreshCategories( db );
            }
            return _categories;
        }

        public static IList<PaymentMethod> GetPaymentMethods( zExpenseEntities db )
        {
            if ( _paymentMethods == null )
            {
                RefreshPaymentMethods( db );
            }
            return _paymentMethods;
        }

        public static IList<Consumer> GetConsumers( zExpenseEntities db )
        {
            if ( _consumers == null )
            {
                RefreshConsumers( db );
            }
            return _consumers;
        }

        public static void RefreshCategories( zExpenseEntities db )
        {
            _categories = db.Categories.ToList();
        }

        public static void RefreshPaymentMethods( zExpenseEntities db )
        {
            _paymentMethods = db.PaymentMethods.ToList();
        }

        private static void RefreshConsumers( zExpenseEntities db )
        {
            _consumers = db.Consumers.ToList();
        }

    }
}