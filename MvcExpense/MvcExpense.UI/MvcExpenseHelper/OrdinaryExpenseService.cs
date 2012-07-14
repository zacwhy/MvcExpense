using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MvcExpense.Core;
using MvcExpense.Core.Models;
using MvcExpense.UI.Models.Input;
using MvcExpense.ViewModels;
using Zac.EnhancedMath;

namespace MvcExpense.Services
{
    public static class OrdinaryExpenseService
    {
        public static List<OrdinaryExpense> GetOrdinaryExpenses( IMvcExpenseUnitOfWork unitOfWork, CreateOrdinaryExpenseInput input, IEnumerable<Category> categories )
        {
            int sequence = unitOfWork.OrdinaryExpenseRepository.NewSequence( input.Date );
            List<OrdinaryExpense> list = GetOrdinaryExpenses( input, sequence, categories );
            return list;
        }

        private static List<OrdinaryExpense> GetOrdinaryExpenses( CreateOrdinaryExpenseInput input, int sequence, IEnumerable<Category> categories )
        {
            var list = new List<OrdinaryExpense>();

            OrdinaryExpense ordinaryExpense = Mapper.Map<CreateOrdinaryExpenseInput, OrdinaryExpense>( input );
            double price = input.Price;
            long[] selectedConsumerIds = input.SelectedConsumerIds;

            int consumerCount = selectedConsumerIds.Count();

            if ( consumerCount == 1 )
            {
                ordinaryExpense.Sequence = sequence;
                ordinaryExpense.ConsumerId = selectedConsumerIds.Single();
                list.Add( ordinaryExpense );
            }
            else if ( consumerCount > 1 )
            {
                double averagePrice = EnhancedMath.RoundDown( price / consumerCount, 2 );
                double primaryConsumerPrice = price - averagePrice * ( consumerCount - 1 );
                Category treat = categories.Single( x => x.Name == "Treat" );
                int i = 0;

                foreach ( long consumerId in selectedConsumerIds )
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

        public static IEnumerable<SelectListGroupItem> GetCategoriesGroupSelectList( IEnumerable<Category> categories )
        {
            IEnumerable<IGrouping<string, Category>> queryLeaves =
                    from x in categories
                    where x.Children.Count == 0
                    group x by x.Parent.Name into grouping
                    select grouping;

            //var list = new List<SelectListGroupItem>();

            foreach ( IGrouping<string, Category> group in queryLeaves )
            {
                var selectListGroupItem = new SelectListGroupItem
                {
                    Name = group.Key,
                    Items = new List<SelectListItem>()
                };

                foreach ( Category category in group )
                {
                    var selectListItem = new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.Id.ToString( CultureInfo.InvariantCulture )
                    };
                    selectListGroupItem.Items.Add( selectListItem );
                }

                yield return selectListGroupItem;
                //list.Add( selectListGroupItem );
            }

            //return list;
        }

    }
}