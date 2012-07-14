using System.Collections.Generic;
using MvcExpense.Core;
using MvcExpense.Core.Models;

namespace MvcExpense.MvcExpenseHelper
{
    public static class ExpenseEntitiesCache
    {
        private static IEnumerable<Category> _categories;
        private static IEnumerable<PaymentMethod> _paymentMethods;
        private static IEnumerable<Consumer> _consumers;

        public static IEnumerable<Category> GetCategories( IMvcExpenseUnitOfWork unitOfWork )
        {
            if ( _categories == null )
            {
                RefreshCategories( unitOfWork );
            }
            return _categories;
        }

        public static IEnumerable<PaymentMethod> GetPaymentMethods( IMvcExpenseUnitOfWork unitOfWork )
        {
            if ( _paymentMethods == null )
            {
                RefreshPaymentMethods( unitOfWork );
            }
            return _paymentMethods;
        }

        public static IEnumerable<Consumer> GetConsumers( IMvcExpenseUnitOfWork unitOfWork )
        {
            if ( _consumers == null )
            {
                RefreshConsumers( unitOfWork );
            }
            return _consumers;
        }

        public static void RefreshCategories( IMvcExpenseUnitOfWork unitOfWork )
        {
            _categories = unitOfWork.CategoryRepository.GetAll();
        }

        public static void RefreshPaymentMethods( IMvcExpenseUnitOfWork unitOfWork )
        {
            _paymentMethods = unitOfWork.PaymentMethodRepository.GetAll();
        }

        public static void RefreshConsumers( IMvcExpenseUnitOfWork unitOfWork )
        {
            _consumers = unitOfWork.ConsumerRepository.GetAll();
        }

    }
}