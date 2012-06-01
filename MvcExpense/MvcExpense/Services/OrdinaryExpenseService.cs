using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MvcExpense.DAL;
using MvcExpense.Models;
using MvcExpense.ViewModels;
using Zac.EnhancedMath;

namespace MvcExpense
{
    public static class OrdinaryExpenseService
    {
        public static DateTime GetMostRecentDate( IMvcExpenseUnitOfWork unitOfWork )
        {
            DateTime date = GetMostRecentDate( unitOfWork.MvcExpenseDbContext );
            return date;
        }

        private static DateTime GetMostRecentDate( MvcExpenseDbContext db )
        {
            IQueryable<DateTime> query =
                from x in db.OrdinaryExpenses
                where x.Id == db.OrdinaryExpenses.Max( y => y.Id )
                select x.Date;
            DateTime date = query.Single();
            return date;
        }

        private static int NewSequence( IMvcExpenseUnitOfWork unitOfWork, DateTime date )
        {
            int sequence = NewSequence( unitOfWork.MvcExpenseDbContext, date );
            return sequence;
        }

        private static int NewSequence( MvcExpenseDbContext db, DateTime date )
        {
            IQueryable<OrdinaryExpense> ordinaryExpenses = db.OrdinaryExpenses.Where( x => x.Date == date );
            int maxSequence = ordinaryExpenses.Max( x => (int?) x.Sequence ) ?? 0;
            return maxSequence + 1;
        }

        public static List<OrdinaryExpense> GetOrdinaryExpenses( IMvcExpenseUnitOfWork unitOfWork, OrdinaryExpenseCreateModel createModel, IList<Category> categories )
        {
            int sequence = NewSequence( unitOfWork, createModel.Date );
            List<OrdinaryExpense> list = GetOrdinaryExpenses( createModel, sequence, categories );
            return list;
        }

        private static List<OrdinaryExpense> GetOrdinaryExpenses( OrdinaryExpenseCreateModel createModel, int sequence, IList<Category> categories )
        {
            List<OrdinaryExpense> list = new List<OrdinaryExpense>();

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
                Category treat = categories.Where( x => x.Name == "Treat" ).Single();
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