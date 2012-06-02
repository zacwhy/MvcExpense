using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MvcExpense.DAL;
using MvcExpense.Models;
using MvcExpense.ViewModels;
using Zac.EnhancedMath;

namespace MvcExpense.Services
{
    public static class OrdinaryExpenseService
    {
        public static DateTime GetMostRecentDate( IMvcExpenseUnitOfWork unitOfWork )
        {
            IQueryable<OrdinaryExpense> query = unitOfWork.OrdinaryExpenseRepository.GetQueryable();

            IQueryable<DateTime> query2 =
                from x in query
                where x.Id == query.Max( y => y.Id )
                select x.Date;

            DateTime date = query2.Single();

            return date;
        }

        private static int NewSequence( IMvcExpenseUnitOfWork unitOfWork, DateTime date )
        {
            IQueryable<OrdinaryExpense> query = unitOfWork.OrdinaryExpenseRepository.GetQueryable();
            query = query.Where( x => x.Date == date );
            int sequence = query.Max( x => (int?) x.Sequence ) ?? 0;
            return sequence + 1;
        }

        public static List<OrdinaryExpense> GetOrdinaryExpenses( IMvcExpenseUnitOfWork unitOfWork, OrdinaryExpenseCreateModel createModel, IEnumerable<Category> categories )
        {
            int sequence = NewSequence( unitOfWork, createModel.Date );
            List<OrdinaryExpense> list = GetOrdinaryExpenses( createModel, sequence, categories );
            return list;
        }

        private static List<OrdinaryExpense> GetOrdinaryExpenses( OrdinaryExpenseCreateModel createModel, int sequence, IEnumerable<Category> categories )
        {
            var list = new List<OrdinaryExpense>();

            OrdinaryExpense ordinaryExpense = Mapper.Map<OrdinaryExpenseCreateModel, OrdinaryExpense>( createModel );
            int consumerCount = createModel.SelectedConsumerIds.Count();

            if ( consumerCount == 1 )
            {
                ordinaryExpense.Sequence = sequence;
                ordinaryExpense.ConsumerId = createModel.SelectedConsumerIds.Single();
                list.Add( ordinaryExpense );
            }
            else if ( consumerCount > 1 )
            {
                double averagePrice = EnhancedMath.RoundDown( createModel.Price / consumerCount, 2 );
                double primaryConsumerPrice = createModel.Price - averagePrice * ( consumerCount - 1 );
                Category treat = categories.Single( x => x.Name == "Treat" );
                int i = 0;

                foreach ( long consumerId in createModel.SelectedConsumerIds )
                {
                    OrdinaryExpense clone = ordinaryExpense.CloneOrdinaryExpense();
                    clone.Sequence = sequence + i;
                    clone.ConsumerId = consumerId;

                    bool isPrimaryConsumer = ( i == 0 );
                    if ( isPrimaryConsumer )
                    {
                        clone.Price = primaryConsumerPrice;
                    }
                    else
                    {
                        clone.CategoryId = treat.Id;
                        clone.Price = averagePrice;
                    }

                    list.Add( clone );
                    i++;
                }
            }

            return list;
        }

    }
}