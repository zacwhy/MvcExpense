using System.Collections.Generic;
using System.Linq;
using MvcExpense.DAL;
using MvcExpense.Models;

namespace MvcExpense.MvcExpenseHelper
{
    public static class ExpenseEntitiesCache
    {
        private static IList<Category> _categories;
        private static IList<PaymentMethod> _paymentMethods;
        private static IList<Consumer> _consumers;

        public static IList<Category> GetCategories( IMvcExpenseUnitOfWork unitOfWork )
        {
            if ( _categories == null )
            {
                RefreshCategories( unitOfWork );
            }
            return _categories;
        }

        public static IList<PaymentMethod> GetPaymentMethods( IMvcExpenseUnitOfWork unitOfWork )
        {
            if ( _paymentMethods == null )
            {
                RefreshPaymentMethods( unitOfWork );
            }
            return _paymentMethods;
        }

        public static IList<Consumer> GetConsumers( IMvcExpenseUnitOfWork unitOfWork )
        {
            if ( _consumers == null )
            {
                RefreshConsumers( unitOfWork );
            }
            return _consumers;
        }

        public static void RefreshCategories( IMvcExpenseUnitOfWork unitOfWork )
        {
            _categories = unitOfWork.CategoryRepository.GetQueryable().ToList();
        }

        public static void RefreshPaymentMethods( IMvcExpenseUnitOfWork unitOfWork )
        {
            _paymentMethods = unitOfWork.PaymentMethodRepository.GetQueryable().ToList();
        }

        public static void RefreshConsumers( IMvcExpenseUnitOfWork unitOfWork )
        {
            _consumers = unitOfWork.ConsumerRepository.GetQueryable().ToList();
        }

    }
}